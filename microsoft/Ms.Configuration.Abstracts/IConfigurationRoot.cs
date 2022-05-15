using System.Collections.Generic;

namespace Ms.Configuration.Abstracts
{
    public interface IConfigurationRoot : IConfiguration
    {
        /// <summary>
        /// ���е�����Provider
        /// </summary>
        IEnumerable<IConfigurationProvider> Providers { get; }
    }
}
