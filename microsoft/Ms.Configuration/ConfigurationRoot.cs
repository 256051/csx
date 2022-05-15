using Microsoft.Extensions.Primitives;
using Ms.Configuration.Abstracts;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Ms.Configuration
{
    public class ConfigurationRoot : IConfigurationRoot, IDisposable
    {
        private readonly IList<IConfigurationProvider> _providers;
        private readonly IList<IDisposable> _changeTokenRegistrations;
        private ConfigurationReloadToken _changeToken = new ConfigurationReloadToken();
        public ConfigurationRoot(IList<IConfigurationProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _providers = providers;
            _changeTokenRegistrations = new List<IDisposable>(providers.Count);
            foreach (IConfigurationProvider p in providers)
            {
                p.Load();
                _changeTokenRegistrations.Add(ChangeToken.OnChange(() => p.GetReloadToken(), () => RaiseChanged()));
            }
        }

        public IEnumerable<IConfigurationProvider> Providers => _providers;

        public string this[string key]
        {
            get => GetConfiguration(_providers, key);
            set => SetConfiguration(_providers, key, value);
        }

        public IConfigurationSection GetSection(string key)
         => new ConfigurationSection(this, key);

        public IChangeToken GetReloadToken() => _changeToken;

        internal static string GetConfiguration(IList<IConfigurationProvider> providers, string key)
        {
            for (int i = providers.Count - 1; i >= 0; i--)
            {
                IConfigurationProvider provider = providers[i];

                if (provider.TryGet(key, out string value))
                {
                    return value;
                }
            }

            return null;
        }

        internal static void SetConfiguration(IList<IConfigurationProvider> providers, string key, string value)
        {
            if (providers.Count == 0)
            {
                throw new InvalidOperationException("");
            }

            foreach (IConfigurationProvider provider in providers)
            {
                provider.Set(key, value);
            }
        }

        public IEnumerable<IConfigurationSection> GetChildren() => this.GetChildrenImplementation(null);

        private void RaiseChanged()
        {
            ConfigurationReloadToken previousToken = Interlocked.Exchange(ref _changeToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
