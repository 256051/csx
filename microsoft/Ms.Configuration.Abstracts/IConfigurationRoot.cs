using System.Collections.Generic;

namespace Ms.Configuration.Abstracts
{
    public interface IConfigurationRoot : IConfiguration
    {
        /// <summary>
        /// ËùÓĞµÄÅäÖÃProvider
        /// </summary>
        IEnumerable<IConfigurationProvider> Providers { get; }
    }
}
