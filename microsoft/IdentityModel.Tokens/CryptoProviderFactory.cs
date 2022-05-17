using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Security.Cryptography;

namespace IdentityModel.Tokens
{
    public class CryptoProviderFactory
    {
        private static CryptoProviderFactory _default;
        static CryptoProviderFactory()
        {
            Default = new CryptoProviderFactory();
        }

        public CryptoProviderFactory()
        {

        }

        public CryptoProviderFactory(CryptoProviderFactory other)
        {
            if (other == null)
                throw new Exception("1");

            CustomCryptoProvider = other.CustomCryptoProvider;
        }

        public static CryptoProviderFactory Default
        {
            get { return _default; }
            set
            {
                _default = value ?? throw new Exception("");
            }
        }

        public ICryptoProvider CustomCryptoProvider { get; set; }

        public virtual bool IsSupportedAlgorithm(string algorithm, SecurityKey key)
        {
            if (CustomCryptoProvider != null && CustomCryptoProvider.IsSupportedAlgorithm(algorithm, key))
                return true;

            return SupportedAlgorithms.IsSupportedAlgorithm(
                        algorithm,
                        (key is JsonWebKey jsonWebKey && jsonWebKey.ConvertedSecurityKey != null)
                        ? jsonWebKey.ConvertedSecurityKey
                        : key);
        }
    }
}
