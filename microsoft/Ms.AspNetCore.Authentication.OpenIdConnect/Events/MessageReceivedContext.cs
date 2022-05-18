// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Ms.AspNetCore.Authentication.OpenIdConnect
{
    /// <summary>
    /// A context for <see cref="OpenIdConnectEvents.OnMessageReceived"/>.
    /// </summary>
    public class MessageReceivedContext : RemoteAuthenticationContext<OpenIdConnectOptions>
    {
        /// <summary>
        /// Initializes a new instance of <see cref="MessageReceivedContext"/>.
        /// </summary>
        /// <inheritdoc />
        public MessageReceivedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            OpenIdConnectOptions options,
            AuthenticationProperties properties)
            : base(context, scheme, options, properties) { }

        /// <summary>
        /// Gets or sets the <see cref="OpenIdConnectMessage"/>.
        /// </summary>
        public OpenIdConnectMessage ProtocolMessage { get; set; }

        /// <summary>
        /// Bearer Token. This will give the application an opportunity to retrieve a token from an alternative location.
        /// </summary>
        public string Token { get; set; }
    }
}
