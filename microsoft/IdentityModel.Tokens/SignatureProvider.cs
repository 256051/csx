using IdentityModel.Logging;
using System;

namespace IdentityModel.Tokens
{
    /// <summary>
    /// Provides signature services, signing and verifying.
    /// </summary>
    public abstract class SignatureProvider : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureProvider"/> class used to create and verify signatures.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to apply.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="algorithm"/> is null or empty.</exception>
        protected SignatureProvider(SecurityKey key, string algorithm)
        {
            Key = key ?? throw LogHelper.LogArgumentNullException(nameof(key));
            Algorithm = (string.IsNullOrEmpty(algorithm)) ? throw LogHelper.LogArgumentNullException(nameof(algorithm)) : algorithm;
        }

        /// <summary>
        /// Gets the signature algorithm.
        /// </summary>
        public string Algorithm { get; private set; }

        /// <summary>
        /// Gets or sets a user context for a <see cref="SignatureProvider"/>.
        /// </summary>
        /// <remarks>This is null by default. This is for use by the application and not used by this SDK.</remarks>
        public string Context { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="CryptoProviderCache"/> that is associated with this <see cref="SignatureProvider"/>
        /// </summary>
        public CryptoProviderCache CryptoProviderCache { get; set; }

        /// <summary>
        /// Calls <see cref="Dispose(bool)"/> and <see cref="GC.SuppressFinalize"/>
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Can be over written in descendants to dispose of internal components.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Gets the <see cref="SecurityKey"/>.
        /// </summary>
        public SecurityKey Key { get; private set; }

        /// <summary>
        /// This must be overridden to produce a signature over the 'input'.
        /// </summary>
        /// <param name="input">bytes to sign.</param>
        /// <returns>signed bytes</returns>
        public abstract byte[] Sign(byte[] input);

        /// Verifies that the <paramref name="signature"/> over <paramref name="input"/> using the
        /// <see cref="SecurityKey"/> and <see cref="SignatureProvider.Algorithm"/> specified by this
        /// <see cref="SignatureProvider"/> are consistent.
        /// <param name="input">the bytes that were signed.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <returns>true if the computed signature matches the signature parameter, false otherwise.</returns>
        public abstract bool Verify(byte[] input, byte[] signature);

        /// <summary>
        /// Gets or sets a bool indicating if this <see cref="SignatureProvider"/> is expected to create signatures.
        /// </summary>
        public bool WillCreateSignatures { get; protected set; }
    }
}
