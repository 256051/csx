using Microsoft.Extensions.Primitives;
using Ms.Configuration.Abstracts;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Ms.Configuration
{
    public abstract class ConfigurationProvider : IConfigurationProvider
    {
        public IDictionary<string, string> Data { get; set; }

        private ConfigurationReloadToken _reloadToken = new ConfigurationReloadToken();

        protected ConfigurationProvider()
        {
            Data = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        }

        public virtual bool TryGet(string key, out string value)
        => Data.TryGetValue(key, out value);

        public virtual void Set(string key, string value)
            => Data[key] = value;

        protected void OnReload()
        {
            ConfigurationReloadToken previousToken = Interlocked.Exchange(ref _reloadToken, new ConfigurationReloadToken());
            previousToken.OnReload();
        }

        public virtual void Load() { }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        private static string Segment(string key, int prefixLength)
        {
            int indexOf = key.IndexOf(ConfigurationPath.KeyDelimiter, prefixLength, StringComparison.OrdinalIgnoreCase);
            return indexOf < 0 ? key.Substring(prefixLength) : key.Substring(prefixLength, indexOf - prefixLength);
        }

        public virtual IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys,string parentPath)
        {
            var results = new List<string>();

            if (parentPath is null)
            {
                foreach (KeyValuePair<string, string> kv in Data)
                {
                    results.Add(Segment(kv.Key, 0));
                }
            }
            else
            {
                Debug.Assert(ConfigurationPath.KeyDelimiter == ":");

                foreach (KeyValuePair<string, string> kv in Data)
                {
                    if (kv.Key.Length > parentPath.Length &&
                        kv.Key.StartsWith(parentPath, StringComparison.OrdinalIgnoreCase) &&
                        kv.Key[parentPath.Length] == ':')
                    {
                        results.Add(Segment(kv.Key, parentPath.Length + 1));
                    }
                }
            }

            results.AddRange(earlierKeys);

            results.Sort(ConfigurationKeyComparer.Comparison);

            return results;
        }
    }
}
