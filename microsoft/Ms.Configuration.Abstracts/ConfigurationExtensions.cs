using System;
using System.Collections.Generic;
using System.Linq;

namespace Ms.Configuration.Abstracts
{
    public static class ConfigurationExtensions
    {
        public static IConfigurationBuilder Add<TSource>(this IConfigurationBuilder builder, Action<TSource> configureSource) where TSource : IConfigurationSource, new()
        {
            var source = new TSource();
            configureSource?.Invoke(source);
            return builder.Add(source);
        }
    }
}
