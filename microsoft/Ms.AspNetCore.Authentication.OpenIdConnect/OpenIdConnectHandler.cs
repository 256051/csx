// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ms.AspNetCore.Authentication.OpenIdConnect
{
    /// <summary>
    /// A per-request authentication handler for the OpenIdConnectAuthenticationMiddleware.
    /// </summary>
    public class OpenIdConnectHandler : RemoteAuthenticationHandler<OpenIdConnectOptions>, IAuthenticationSignOutHandler
    {
        protected HtmlEncoder HtmlEncoder { get; }

        public OpenIdConnectHandler(IOptionsMonitor<OpenIdConnectOptions> options, ILoggerFactory logger, HtmlEncoder htmlEncoder, UrlEncoder encoder, ISystemClock clock)
    : base(options, logger, encoder, clock)
        {
            HtmlEncoder = htmlEncoder;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            OpenIdConnectMessage authorizationResponse = null;

            if (HttpMethods.IsGet(Request.Method))
            {
                authorizationResponse = new OpenIdConnectMessage(Request.Query.Select(pair => new KeyValuePair<string, string[]>(pair.Key, pair.Value)));

                // response_mode=query (explicit or not) and a response_type containing id_token
                // or token are not considered as a safe combination and MUST be rejected.
                // See http://openid.net/specs/oauth-v2-multiple-response-types-1_0.html#Security
                if (!string.IsNullOrEmpty(authorizationResponse.IdToken) || !string.IsNullOrEmpty(authorizationResponse.AccessToken))
                {
                    if (Options.SkipUnrecognizedRequests)
                    {
                        // Not for us?
                        return HandleRequestResult.SkipHandler();
                    }
                    return HandleRequestResult.Fail("An OpenID Connect response cannot contain an " +
                            "identity token or an access token when using response_mode=query");
                }
            }
            // assumption: if the ContentType is "application/x-www-form-urlencoded" it should be safe to read as it is small.
            else if (HttpMethods.IsPost(Request.Method)
              && !string.IsNullOrEmpty(Request.ContentType)
              // May have media/type; charset=utf-8, allow partial match.
              && Request.ContentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase)
              && Request.Body.CanRead)
            {
                var form = await Request.ReadFormAsync();
                authorizationResponse = new OpenIdConnectMessage(form.Select(pair => new KeyValuePair<string, string[]>(pair.Key, pair.Value)));
            }

            if (authorizationResponse == null)
            {
                if (Options.SkipUnrecognizedRequests)
                {
                    // Not for us?
                    return HandleRequestResult.SkipHandler();
                }
                return HandleRequestResult.Fail("No message.");
            }

            AuthenticationProperties properties = null;
            try
            {
                properties = ReadPropertiesAndClearState(authorizationResponse);

                var messageReceivedContext = await RunMessageReceivedEventAsync(authorizationResponse, properties);
                if (messageReceivedContext.Result != null)
                {
                    return messageReceivedContext.Result;
                }
                authorizationResponse = messageReceivedContext.ProtocolMessage;
                properties = messageReceivedContext.Properties;

                if (properties == null || properties.Items.Count == 0)
                {
                    // Fail if state is missing, it's required for the correlation id.
                    if (string.IsNullOrEmpty(authorizationResponse.State))
                    {
                        // This wasn't a valid OIDC message, it may not have been intended for us.
                        Logger.NullOrEmptyAuthorizationResponseState();
                        if (Options.SkipUnrecognizedRequests)
                        {
                            return HandleRequestResult.SkipHandler();
                        }
                        return HandleRequestResult.Fail(Resources.MessageStateIsNullOrEmpty);
                    }

                    properties = ReadPropertiesAndClearState(authorizationResponse);
                }

                if (properties == null)
                {
                    Logger.UnableToReadAuthorizationResponseState();
                    if (Options.SkipUnrecognizedRequests)
                    {
                        // Not for us?
                        return HandleRequestResult.SkipHandler();
                    }

                    // if state exists and we failed to 'unprotect' this is not a message we should process.
                    return HandleRequestResult.Fail(Resources.MessageStateIsInvalid);
                }

                if (!ValidateCorrelationId(properties))
                {
                    return HandleRequestResult.Fail("Correlation failed.", properties);
                }

                // if any of the error fields are set, throw error null
                if (!string.IsNullOrEmpty(authorizationResponse.Error))
                {
                    // Note: access_denied errors are special protocol errors indicating the user didn't
                    // approve the authorization demand requested by the remote authorization server.
                    // Since it's a frequent scenario (that is not caused by incorrect configuration),
                    // denied errors are handled differently using HandleAccessDeniedErrorAsync().
                    // Visit https://tools.ietf.org/html/rfc6749#section-4.1.2.1 for more information.
                    if (string.Equals(authorizationResponse.Error, "access_denied", StringComparison.Ordinal))
                    {
                        var result = await HandleAccessDeniedErrorAsync(properties);
                        if (!result.None)
                        {
                            return result;
                        }
                    }

                    return HandleRequestResult.Fail(CreateOpenIdConnectProtocolException(authorizationResponse, response: null), properties);
                }

                if (_configuration == null && Options.ConfigurationManager != null)
                {
                    Logger.UpdatingConfiguration();
                    _configuration = await Options.ConfigurationManager.GetConfigurationAsync(Context.RequestAborted);
                }

                PopulateSessionProperties(authorizationResponse, properties);

                ClaimsPrincipal user = null;
                JwtSecurityToken jwt = null;
                string nonce = null;
                var validationParameters = Options.TokenValidationParameters.Clone();

                // Hybrid or Implicit flow
                if (!string.IsNullOrEmpty(authorizationResponse.IdToken))
                {
                    Logger.ReceivedIdToken();
                    user = ValidateToken(authorizationResponse.IdToken, properties, validationParameters, out jwt);

                    nonce = jwt.Payload.Nonce;
                    if (!string.IsNullOrEmpty(nonce))
                    {
                        nonce = ReadNonceCookie(nonce);
                    }

                    var tokenValidatedContext = await RunTokenValidatedEventAsync(authorizationResponse, null, user, properties, jwt, nonce);
                    if (tokenValidatedContext.Result != null)
                    {
                        return tokenValidatedContext.Result;
                    }
                    authorizationResponse = tokenValidatedContext.ProtocolMessage;
                    user = tokenValidatedContext.Principal;
                    properties = tokenValidatedContext.Properties;
                    jwt = tokenValidatedContext.SecurityToken;
                    nonce = tokenValidatedContext.Nonce;
                }

                Options.ProtocolValidator.ValidateAuthenticationResponse(new OpenIdConnectProtocolValidationContext()
                {
                    ClientId = Options.ClientId,
                    ProtocolMessage = authorizationResponse,
                    ValidatedIdToken = jwt,
                    Nonce = nonce
                });

                OpenIdConnectMessage tokenEndpointResponse = null;

                // Authorization Code or Hybrid flow
                if (!string.IsNullOrEmpty(authorizationResponse.Code))
                {
                    var authorizationCodeReceivedContext = await RunAuthorizationCodeReceivedEventAsync(authorizationResponse, user, properties, jwt);
                    if (authorizationCodeReceivedContext.Result != null)
                    {
                        return authorizationCodeReceivedContext.Result;
                    }
                    authorizationResponse = authorizationCodeReceivedContext.ProtocolMessage;
                    user = authorizationCodeReceivedContext.Principal;
                    properties = authorizationCodeReceivedContext.Properties;
                    var tokenEndpointRequest = authorizationCodeReceivedContext.TokenEndpointRequest;
                    // If the developer redeemed the code themselves...
                    tokenEndpointResponse = authorizationCodeReceivedContext.TokenEndpointResponse;
                    jwt = authorizationCodeReceivedContext.JwtSecurityToken;

                    if (!authorizationCodeReceivedContext.HandledCodeRedemption)
                    {
                        tokenEndpointResponse = await RedeemAuthorizationCodeAsync(tokenEndpointRequest);
                    }

                    var tokenResponseReceivedContext = await RunTokenResponseReceivedEventAsync(authorizationResponse, tokenEndpointResponse, user, properties);
                    if (tokenResponseReceivedContext.Result != null)
                    {
                        return tokenResponseReceivedContext.Result;
                    }

                    authorizationResponse = tokenResponseReceivedContext.ProtocolMessage;
                    tokenEndpointResponse = tokenResponseReceivedContext.TokenEndpointResponse;
                    user = tokenResponseReceivedContext.Principal;
                    properties = tokenResponseReceivedContext.Properties;

                    // no need to validate signature when token is received using "code flow" as per spec
                    // [http://openid.net/specs/openid-connect-core-1_0.html#IDTokenValidation].
                    validationParameters.RequireSignedTokens = false;

                    // At least a cursory validation is required on the new IdToken, even if we've already validated the one from the authorization response.
                    // And we'll want to validate the new JWT in ValidateTokenResponse.
                    var tokenEndpointUser = ValidateToken(tokenEndpointResponse.IdToken, properties, validationParameters, out var tokenEndpointJwt);

                    // Avoid reading & deleting the nonce cookie, running the event, etc, if it was already done as part of the authorization response validation.
                    if (user == null)
                    {
                        nonce = tokenEndpointJwt.Payload.Nonce;
                        if (!string.IsNullOrEmpty(nonce))
                        {
                            nonce = ReadNonceCookie(nonce);
                        }

                        var tokenValidatedContext = await RunTokenValidatedEventAsync(authorizationResponse, tokenEndpointResponse, tokenEndpointUser, properties, tokenEndpointJwt, nonce);
                        if (tokenValidatedContext.Result != null)
                        {
                            return tokenValidatedContext.Result;
                        }
                        authorizationResponse = tokenValidatedContext.ProtocolMessage;
                        tokenEndpointResponse = tokenValidatedContext.TokenEndpointResponse;
                        user = tokenValidatedContext.Principal;
                        properties = tokenValidatedContext.Properties;
                        jwt = tokenValidatedContext.SecurityToken;
                        nonce = tokenValidatedContext.Nonce;
                    }
                    else
                    {
                        if (!string.Equals(jwt.Subject, tokenEndpointJwt.Subject, StringComparison.Ordinal))
                        {
                            throw new SecurityTokenException("The sub claim does not match in the id_token's from the authorization and token endpoints.");
                        }

                        jwt = tokenEndpointJwt;
                    }

                    // Validate the token response if it wasn't provided manually
                    if (!authorizationCodeReceivedContext.HandledCodeRedemption)
                    {
                        Options.ProtocolValidator.ValidateTokenResponse(new OpenIdConnectProtocolValidationContext()
                        {
                            ClientId = Options.ClientId,
                            ProtocolMessage = tokenEndpointResponse,
                            ValidatedIdToken = jwt,
                            Nonce = nonce
                        });
                    }
                }

                if (Options.SaveTokens)
                {
                    SaveTokens(properties, tokenEndpointResponse ?? authorizationResponse);
                }

                if (Options.GetClaimsFromUserInfoEndpoint)
                {
                    return await GetUserInformationAsync(tokenEndpointResponse ?? authorizationResponse, jwt, user, properties);
                }
                else
                {
                    using (var payload = JsonDocument.Parse("{}"))
                    {
                        var identity = (ClaimsIdentity)user.Identity;
                        foreach (var action in Options.ClaimActions)
                        {
                            action.Run(payload.RootElement, identity, ClaimsIssuer);
                        }
                    }
                }

                return HandleRequestResult.Success(new AuthenticationTicket(user, properties, Scheme.Name));
            }
            catch (Exception exception)
            {
                Logger.ExceptionProcessingMessage(exception);

                // Refresh the configuration for exceptions that may be caused by key rollovers. The user can also request a refresh in the event.
                if (Options.RefreshOnIssuerKeyNotFound && exception is SecurityTokenSignatureKeyNotFoundException)
                {
                    if (Options.ConfigurationManager != null)
                    {
                        Logger.ConfigurationManagerRequestRefreshCalled();
                        Options.ConfigurationManager.RequestRefresh();
                    }
                }

                var authenticationFailedContext = await RunAuthenticationFailedEventAsync(authorizationResponse, exception);
                if (authenticationFailedContext.Result != null)
                {
                    return authenticationFailedContext.Result;
                }

                return HandleRequestResult.Fail(exception, properties);
            }
        }
    }
}
