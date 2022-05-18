
using System;
using System.Security.Cryptography;
using IdentityModel.Logging;

namespace IdentityModel.Tokens
{
    /// <summary>
    /// Provides signing and verifying operations using a <see cref="SymmetricSecurityKey"/> and specifying an algorithm.
    /// </summary>
    public class SymmetricSignatureProvider : SignatureProvider
    {
        private bool _disposed;
        private KeyedHashAlgorithm _keyedHash;
        private object _signLock = new object();
        private object _verifyLock = new object();

        /// <summary>
        /// This is the minimum <see cref="SymmetricSecurityKey"/>.KeySize when creating and verifying signatures.
        /// </summary>
        public static readonly int DefaultMinimumSymmetricKeySizeInBits = 128;

        private int _minimumSymmetricKeySizeInBits = DefaultMinimumSymmetricKeySizeInBits;

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricSignatureProvider"/> class that uses an <see cref="SecurityKey"/> to create and / or verify signatures over a array of bytes.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to use.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        /// <exception cref="ArgumentNullException">'algorithm' is null or empty.</exception>
        /// <exception cref="NotSupportedException">If <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="ArgumentOutOfRangeException">'<see cref="SecurityKey"/>.KeySize' is smaller than <see cref="SymmetricSignatureProvider.MinimumSymmetricKeySizeInBits"/>.</exception>
        public SymmetricSignatureProvider(SecurityKey key, string algorithm)
            : this(key, algorithm, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SymmetricSignatureProvider"/> class that uses an <see cref="SecurityKey"/> to create and / or verify signatures over a array of bytes.
        /// </summary>
        /// <param name="key">The <see cref="SecurityKey"/> that will be used for signature operations.</param>
        /// <param name="algorithm">The signature algorithm to use.</param>
        /// <param name="willCreateSignatures">indicates if this <see cref="SymmetricSignatureProvider"/> will be used to create signatures.</param>
        /// <exception cref="ArgumentNullException">'key' is null.</exception>
        /// <exception cref="ArgumentNullException">'algorithm' is null or empty.</exception>
        /// <exception cref="NotSupportedException">If <see cref="SecurityKey"/> and algorithm pair are not supported.</exception>
        /// <exception cref="ArgumentOutOfRangeException">'<see cref="SecurityKey"/>.KeySize' is smaller than <see cref="SymmetricSignatureProvider.MinimumSymmetricKeySizeInBits"/>.</exception>
        public SymmetricSignatureProvider(SecurityKey key, string algorithm, bool willCreateSignatures)
            : base(key, algorithm)
        {
            if (!key.CryptoProviderFactory.IsSupportedAlgorithm(algorithm, key))
                throw LogHelper.LogExceptionMessage(new NotSupportedException(LogHelper.FormatInvariant(LogMessages.IDX10634, (algorithm ?? "null"), key)));

            if (key.KeySize < MinimumSymmetricKeySizeInBits)
                throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(key), LogHelper.FormatInvariant(LogMessages.IDX10653, (algorithm ?? "null"), MinimumSymmetricKeySizeInBits, key, key.KeySize)));

            WillCreateSignatures = willCreateSignatures;
        }

        /// <summary>
        /// Gets or sets the minimum <see cref="SymmetricSecurityKey"/>.KeySize"/>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">'value' is smaller than <see cref="DefaultMinimumSymmetricKeySizeInBits"/>.</exception>
        public int MinimumSymmetricKeySizeInBits
        {
            get
            {
                return _minimumSymmetricKeySizeInBits;
            }
            set
            {
                if (value < DefaultMinimumSymmetricKeySizeInBits)
                    throw LogHelper.LogExceptionMessage(new ArgumentOutOfRangeException(nameof(value), LogHelper.FormatInvariant(LogMessages.IDX10628, DefaultMinimumSymmetricKeySizeInBits)));

                _minimumSymmetricKeySizeInBits = value;
            }
        }

        /// <summary>
        /// Called to obtain the byte[] needed to create a <see cref="KeyedHashAlgorithm"/>
        /// </summary>
        /// <param name="key"><see cref="SecurityKey"/>that will be used to obtain the byte[].</param>
        /// <returns><see cref="byte"/>[] that is used to populated the KeyedHashAlgorithm.</returns>
        /// <exception cref="ArgumentNullException">if key is null.</exception>
        /// <exception cref="ArgumentException">if a byte[] can not be obtained from SecurityKey.</exception>
        /// <remarks><see cref="SymmetricSecurityKey"/> and <see cref="JsonWebKey"/> are supported.
        /// <para>For a <see cref="SymmetricSecurityKey"/> .Key is returned</para>
        /// <para>For a <see cref="JsonWebKey"/>Base64UrlEncoder.DecodeBytes is called with <see cref="JsonWebKey.K"/> if <see cref="JsonWebKey.Kty"/> == JsonWebAlgorithmsKeyTypes.Octet</para>
        /// </remarks>
        protected virtual byte[] GetKeyBytes(SecurityKey key)
        {
            if (key == null)
                throw LogHelper.LogArgumentNullException(nameof(key));

            if (key is SymmetricSecurityKey symmetricSecurityKey)
                return symmetricSecurityKey.Key;

            if (key is JsonWebKey jsonWebKey && jsonWebKey.K != null && jsonWebKey.Kty == JsonWebAlgorithmsKeyTypes.Octet)
                return Base64UrlEncoder.DecodeBytes(jsonWebKey.K);

            throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10667, key)));
        }

        /// <summary>
        /// Returns the <see cref="KeyedHashAlgorithm"/>.
        /// </summary>
        /// <param name="algorithm">The hash algorithm to use to create the hash value.</param>
        /// <param name="keyBytes">The byte array of the key.</param>
        /// <returns></returns>
        protected virtual KeyedHashAlgorithm GetKeyedHashAlgorithm(byte[] keyBytes, string algorithm)
        {
            if (_keyedHash == null)
            {
                try
                {
                    _keyedHash = Key.CryptoProviderFactory.CreateKeyedHashAlgorithm(keyBytes, algorithm);
                }
                catch (Exception ex)
                {
                    throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10677, Key, (algorithm ?? "null")), ex));
                }
            }

            return _keyedHash;
        }

        /// <summary>
        /// Gets the <see cref="KeyedHashAlgorithm"/> for this <see cref="SymmetricSignatureProvider"/>.
        /// </summary>
        private KeyedHashAlgorithm KeyedHashAlgorithm
        {
            get
            {
                if (_keyedHash == null)
                {
                    try
                    {
                        _keyedHash = GetKeyedHashAlgorithm(GetKeyBytes(Key), Algorithm);
                    }
                    catch(Exception ex)
                    {
                        throw LogHelper.LogExceptionMessage(new InvalidOperationException(LogHelper.FormatInvariant(LogMessages.IDX10677, Key, (Algorithm ?? "null")), ex));
                    }
                }

                return _keyedHash;
            }
        }

        /// <summary>
        /// Produces a signature over the 'input' using the <see cref="SymmetricSecurityKey"/> and 'algorithm' passed to <see cref="SymmetricSignatureProvider( SecurityKey, string )"/>.
        /// </summary>
        /// <param name="input">The bytes to sign.</param>
        /// <returns>Signed bytes</returns>
        /// <exception cref="ArgumentNullException">'input' is null. </exception>
        /// <exception cref="ArgumentException">'input.Length' == 0. </exception>
        /// <exception cref="ObjectDisposedException"><see cref="Dispose(bool)"/> has been called.</exception>
        /// <exception cref="InvalidOperationException"><see cref="KeyedHashAlgorithm"/> is null. This can occur if a derived type deletes it or does not create it.</exception>
        /// <remarks>Sign is thread safe.</remarks>
        public override byte[] Sign(byte[] input)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            LogHelper.LogInformation(LogMessages.IDX10642, input);

            try
            {
                lock (_signLock)
                {
                    return KeyedHashAlgorithm.ComputeHash(input);
                }
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
        }

        /// <summary>
        /// Verifies that a signature created over the 'input' matches the signature. Using <see cref="SymmetricSecurityKey"/> and 'algorithm' passed to <see cref="SymmetricSignatureProvider( SecurityKey, string )"/>.
        /// </summary>
        /// <param name="input">The bytes to verify.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <returns>true if computed signature matches the signature parameter, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">'input' is null.</exception>
        /// <exception cref="ArgumentNullException">'signature' is null.</exception>
        /// <exception cref="ArgumentException">'input.Length' == 0.</exception>
        /// <exception cref="ArgumentException">'signature.Length' == 0. </exception>
        /// <exception cref="ObjectDisposedException"><see cref="Dispose(bool)"/> has been called.</exception>
        /// <exception cref="InvalidOperationException">If the internal <see cref="KeyedHashAlgorithm"/> is null. This can occur if a derived type deletes it or does not create it.</exception>
        /// <remarks>Verify is thread safe.</remarks>
        public override bool Verify(byte[] input, byte[] signature)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }


            LogHelper.LogInformation(LogMessages.IDX10643, input);
            try
            {
                lock (_verifyLock)
                {
                    return Utility.AreEqual(signature, KeyedHashAlgorithm.ComputeHash(input));
                }
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
        }

        /// <summary>
        /// Verifies that a signature created over the 'input' matches the signature. Using <see cref="SymmetricSecurityKey"/> and 'algorithm' passed to <see cref="SymmetricSignatureProvider( SecurityKey, string )"/>.
        /// </summary>
        /// <param name="input">The bytes to verify.</param>
        /// <param name="signature">signature to compare against.</param>
        /// <param name="length">number of bytes of signature to use.</param>
        /// <returns>true if computed signature matches the signature parameter, false otherwise.</returns>
        /// <exception cref="ArgumentNullException">'input' is null.</exception>
        /// <exception cref="ArgumentNullException">'signature' is null.</exception>
        /// <exception cref="ArgumentException">'input.Length' == 0.</exception>
        /// <exception cref="ArgumentException">'signature.Length' == 0. </exception>
        /// <exception cref="ArgumentException">'length &lt; 1'</exception>
        /// <exception cref="ObjectDisposedException"><see cref="Dispose(bool)"/> has been called.</exception>
        /// <exception cref="InvalidOperationException">If the internal <see cref="KeyedHashAlgorithm"/> is null. This can occur if a derived type deletes it or does not create it.</exception>
        public bool Verify(byte[] input, byte[] signature, int length)
        {
            if (input == null || input.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(input));

            if (signature == null || signature.Length == 0)
                throw LogHelper.LogArgumentNullException(nameof(signature));

            if (length < 1)
                throw LogHelper.LogExceptionMessage(new ArgumentException(LogHelper.FormatInvariant(LogMessages.IDX10655, length)));

            if (_disposed)
            {
                CryptoProviderCache?.TryRemove(this);
                throw LogHelper.LogExceptionMessage(new ObjectDisposedException(GetType().ToString()));
            }

            LogHelper.LogInformation(LogMessages.IDX10643, input);
            try
            {
                lock (_verifyLock)
                {
                    return Utility.AreEqual(signature, KeyedHashAlgorithm.ComputeHash(input), length);
                }
            }
            catch
            {
                CryptoProviderCache?.TryRemove(this);
                throw;
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Disposes of internal components.
        /// </summary>
        /// <param name="disposing">true, if called from Dispose(), false, if invoked inside a finalizer.</param>
        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;
                CryptoProviderCache?.TryRemove(this);

                if (disposing)
                {
                    if (_keyedHash != null)
                    {
                        _keyedHash.Dispose();
                        _keyedHash = null;
                    }
                }
            }
        }

        #endregion
    }
}
