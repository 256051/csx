using System;
using System.ComponentModel;
using static IdentityModel.Logging.LogHelper;

namespace IdentityModel.Tokens
{
    /// <summary>
    /// Defines properties shared across all security token handlers.
    /// </summary>
    public abstract class TokenHandler
    {
        private int _defaultTokenLifetimeInMinutes = DefaultTokenLifetimeInMinutes;
        private int _maximumTokenSizeInBytes = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        /// <summary>
        /// Default lifetime of tokens created. When creating tokens, if 'expires' and 'notbefore' are both null, 
        /// then a default will be set to: expires = DateTime.UtcNow, notbefore = DateTime.UtcNow + TimeSpan.FromMinutes(TokenLifetimeInMinutes).
        /// </summary>
        public static readonly int DefaultTokenLifetimeInMinutes = 60;

        /// <summary>
        /// Gets and sets the maximum token size in bytes that will be processed.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">'value' less than 1.</exception>
        public virtual int MaximumTokenSizeInBytes
        {
            get => _maximumTokenSizeInBytes; 
            set => _maximumTokenSizeInBytes =  (value < 1) ? throw LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), FormatInvariant(LogMessages.IDX10101, value))) : value;
        }

        /// <summary>
        /// Gets or sets a bool that controls if token creation will set default 'exp', 'nbf' and 'iat' if not specified.
        /// </summary>
        /// <remarks>See: <see cref="TokenLifetimeInMinutes"/> for configuration.</remarks>
        [DefaultValue(true)]
        public bool SetDefaultTimesOnTokenCreation { get; set; } = true;

        /// <summary>
        /// Gets or sets the token lifetime in minutes.
        /// </summary>
        /// <remarks>Used during token creation to set the default expiration ('exp'). </remarks>
        /// <exception cref="ArgumentOutOfRangeException">'value' less than 1.</exception>
        public int TokenLifetimeInMinutes
        {
            get => _defaultTokenLifetimeInMinutes;
            set => _defaultTokenLifetimeInMinutes = (value < 1) ? throw LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), FormatInvariant(LogMessages.IDX10104, value))) : value;
        }
    }
}
