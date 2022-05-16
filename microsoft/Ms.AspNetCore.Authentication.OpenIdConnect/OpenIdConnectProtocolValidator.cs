using IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Ms.AspNetCore.Authentication.OpenIdConnect
{
    public class OpenIdConnectProtocolValidator
    {
        private CryptoProviderFactory _cryptoProviderFactory;
        private TimeSpan _nonceLifetime = DefaultNonceLifetime;

        public static readonly TimeSpan DefaultNonceLifetime = TimeSpan.FromMinutes(60);
        public OpenIdConnectProtocolValidator()
        {
            RequireAcr = false;
            RequireAmr = false;
            RequireAuthTime = false;
            RequireAzp = false;
            RequireNonce = true;
            RequireState = true;
            RequireTimeStampInNonce = true;
            RequireStateValidation = true;
            _cryptoProviderFactory = new CryptoProviderFactory(CryptoProviderFactory.Default);
        }

        /// <summary>
        /// Gets or sets a value indicating if an 'acr' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAcr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if an 'amr' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAmr { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if an 'auth_time' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAuthTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if an 'azp' claim is required.
        /// </summary>
        [DefaultValue(false)]
        public bool RequireAzp { get; set; }

        /// <summary>
        /// Get or sets if a nonce is required.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireNonce { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if a 'state' is required.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireState { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if validation of 'state' is turned on or off.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireStateValidation { get; set; }

        /// <summary>
        /// Gets or sets a value indicating if a 'sub' claim is required.
        /// </summary>
        [DefaultValue(true)]
        public bool RequireSub { get; set; } = RequireSubByDefault;

        /// <summary>
        /// Gets or sets a value for default RequreSub.
        /// </summary>
        /// <remarks>default: true.</remarks>
        public static bool RequireSubByDefault { get; set; } = true;

        /// <summary>
        /// Gets or set logic to control if a nonce is prefixed with a timestamp.
        /// </summary>
        /// <remarks>if <see cref="RequireTimeStampInNonce"/> is true then:
        /// <para><see cref="GenerateNonce"/> will return a 'nonce' with the Epoch time as the prefix, delimited with a '.'.</para>
        /// <para><see cref="ValidateNonce"/> will require that the 'nonce' has a valid time as the prefix.</para>
        /// </remarks>
        [DefaultValue(true)]
        public bool RequireTimeStampInNonce { get; set; }

        public TimeSpan NonceLifetime
        {
            get
            {
                return _nonceLifetime;
            }

            set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new Exception("");
                }

                _nonceLifetime = value;
            }
        }
    }
}
