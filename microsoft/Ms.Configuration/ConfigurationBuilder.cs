using Ms.Configuration.Abstracts;
using System;
using System.Collections.Generic;

namespace Ms.Configuration
{
    /// <summary>
    /// 配置生成器
    /// </summary>
    public class ConfigurationBuilder : IConfigurationBuilder
    {
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        public IList<IConfigurationSource> Sources { get; } = new List<IConfigurationSource>();

        public IConfigurationBuilder Add(IConfigurationSource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sources.Add(source);
            return this;
        }

        public IConfigurationRoot Build()
        {
            var providers = new List<IConfigurationProvider>();
            foreach (IConfigurationSource source in Sources)
            {
                IConfigurationProvider provider = source.Build(this);
                providers.Add(provider);
            }
            return new ConfigurationRoot(providers);
        }
    }
}
