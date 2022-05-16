using System;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityModel.Protocols
{
    /// <summary>
    /// This type is for users that want a fixed and static Configuration.
    /// In this case, the configuration is obtained and passed to the constructor.
    /// </summary>
    /// <typeparam name="T">must be a class.</typeparam>
    public class StaticConfigurationManager<T> : IConfigurationManager<T> where T : class
    {
        private T _configuration;

        /// <summary>
        /// Initializes an new instance of <see cref="StaticConfigurationManager{T}"/> with a Configuration instance.
        /// </summary>
        /// <param name="configuration">Configuration of type OpenIdConnectConfiguration or OpenIdConnectConfiguration.</param>
        public StaticConfigurationManager(T configuration)
        {
            if (configuration == null)
                throw new Exception("");

            _configuration = configuration;
        }

        /// <summary>
        /// Obtains an updated version of Configuration.
        /// </summary>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>Configuration of type T.</returns>
        public Task<T> GetConfigurationAsync(CancellationToken cancel)
        {
            return Task.FromResult(_configuration);
        }

        /// <summary>
        /// For the this type, this is a no-op
        /// </summary>
        public void RequestRefresh()
        {
        }
    }
}
