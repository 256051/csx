// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using IdentityModel.Protocols;
using IdentityModel.Protocols.OpenIdConnect;
using IdentityModel.Tokens;
using IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace Ms.AspNetCore.Authentication.OpenIdConnect
{
    /// <summary>
    /// Configuration options for <see cref="OpenIdConnectHandler"/>
    /// </summary>
    public class OpenIdConnectOptions : RemoteAuthenticationOptions
    {
        private JwtSecurityTokenHandler _defaultHandler = new JwtSecurityTokenHandler();
        private CookieBuilder _nonceCookieBuilder;

        public OpenIdConnectOptions()
        {
            CallbackPath = new PathString("/signin-oidc");
            SignedOutCallbackPath = new PathString("/signout-callback-oidc");
            RemoteSignOutPath = new PathString("/signout-oidc");
            SecurityTokenValidator = _defaultHandler;

            Events = new OpenIdConnectEvents();
            Scope.Add("openid");
            Scope.Add("profile");

            ClaimActions.DeleteClaim("nonce");
            ClaimActions.DeleteClaim("aud");
            ClaimActions.DeleteClaim("azp");
            ClaimActions.DeleteClaim("acr");
            ClaimActions.DeleteClaim("iss");
            ClaimActions.DeleteClaim("iat");
            ClaimActions.DeleteClaim("nbf");
            ClaimActions.DeleteClaim("exp");
            ClaimActions.DeleteClaim("at_hash");
            ClaimActions.DeleteClaim("c_hash");
            ClaimActions.DeleteClaim("ipaddr");
            ClaimActions.DeleteClaim("platf");
            ClaimActions.DeleteClaim("ver");

            // http://openid.net/specs/openid-connect-core-1_0.html#StandardClaims
            ClaimActions.MapUniqueJsonKey("sub", "sub");
            ClaimActions.MapUniqueJsonKey("name", "name");
            ClaimActions.MapUniqueJsonKey("given_name", "given_name");
            ClaimActions.MapUniqueJsonKey("family_name", "family_name");
            ClaimActions.MapUniqueJsonKey("profile", "profile");
            ClaimActions.MapUniqueJsonKey("email", "email");

            _nonceCookieBuilder = new OpenIdConnectNonceCookieBuilder(this)
            {
                Name = OpenIdConnectDefaults.CookieNoncePrefix,
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                SecurePolicy = CookieSecurePolicy.SameAsRequest,
                IsEssential = true,
            };
        }

        /// <summary>
        /// ??????????????????????????????????????????
        /// </summary>
        public PathString SignedOutCallbackPath { get; set; }

        public PathString RemoteSignOutPath { get; set; }

        /// <summary>
        /// ????????????
        /// </summary>
        public ISecurityTokenValidator SecurityTokenValidator { get; set; }

        public ICollection<string> Scope { get; } = new HashSet<string>();

        public ClaimActionCollection ClaimActions { get; } = new ClaimActionCollection();

        /// <summary>
        /// Gets or sets the 'client_secret'.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// ????????
        /// </summary>
        public OpenIdConnectProtocolValidator ProtocolValidator { get; set; } = new OpenIdConnectProtocolValidator()
        {
            RequireStateValidation = false,
            NonceLifetime = TimeSpan.FromMinutes(15)
        };

        public string SignOutScheme { get; set; }

        public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

        public ISecureDataFormat<string> StringDataFormat { get; set; }

        /// <summary>
        /// ??????id
        /// </summary>
        public string ClientId { get; set; }

        public bool DisableTelemetry { get; set; }

        private class OpenIdConnectNonceCookieBuilder : RequestPathBaseCookieBuilder
        {
            private readonly OpenIdConnectOptions _options;

            public OpenIdConnectNonceCookieBuilder(OpenIdConnectOptions oidcOptions)
            {
                _options = oidcOptions;
            }

            protected override string AdditionalPath => _options.CallbackPath;

            public override CookieOptions Build(HttpContext context, DateTimeOffset expiresFrom)
            {
                var cookieOptions = base.Build(context, expiresFrom);

                if (!Expiration.HasValue || !cookieOptions.Expires.HasValue)
                {
                    cookieOptions.Expires = expiresFrom.Add(_options.ProtocolValidator.NonceLifetime);
                }

                return cookieOptions;
            }
        }

        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();

        public IConfigurationManager<OpenIdConnectConfiguration> ConfigurationManager { get; set; }

        public OpenIdConnectConfiguration Configuration { get; set; }

        public bool UseTokenLifetime { get; set; }

        /// <summary>
        /// ????????????????????????????????????
        /// </summary>
        public string MetadataAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// https????
        /// </summary>
        public bool RequireHttpsMetadata { get; set; } = true;

        public bool GetClaimsFromUserInfoEndpoint { get; set; }

        public bool RefreshOnIssuerKeyNotFound { get; set; } = true;

        /// <summary>
        /// ??????????????????
        /// </summary>
        public TimeSpan RefreshInterval { get; set; } = ConfigurationManager<OpenIdConnectConfiguration>.DefaultRefreshInterval;

        public TimeSpan AutomaticRefreshInterval { get; set; } = ConfigurationManager<OpenIdConnectConfiguration>.DefaultAutomaticRefreshInterval;

        public bool SkipUnrecognizedRequests { get; set; } = false;

        public CookieBuilder NonceCookie
        {
            get => _nonceCookieBuilder;
            set => _nonceCookieBuilder = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}
