using Framework.Core;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Framework.Json
{
    /// <summary>
    /// newton序列化和system.text.json联合序列化
    /// </summary>
    public class HybridJsonSerializer : IJsonSerializer, ITransient
    {
        protected JsonOptions Options { get; }

        protected IServiceScopeFactory ServiceScopeFactory { get; }
        public HybridJsonSerializer(IOptions<JsonOptions> options, IServiceScopeFactory serviceScopeFactory)
        {
            Options = options.Value;
            ServiceScopeFactory = serviceScopeFactory;
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var serializerProvider = GetSerializerProvider(scope.ServiceProvider, obj?.GetType());
                return serializerProvider.Serialize(obj, camelCase, indented);
            }
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            Check.NotNull(jsonString, nameof(jsonString));

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var serializerProvider = GetSerializerProvider(scope.ServiceProvider, typeof(T));
                return serializerProvider.Deserialize<T>(jsonString, camelCase);
            }
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            Check.NotNull(jsonString, nameof(jsonString));

            using (var scope = ServiceScopeFactory.CreateScope())
            {
                var serializerProvider = GetSerializerProvider(scope.ServiceProvider, type);
                return serializerProvider.Deserialize(type, jsonString, camelCase);
            }
        }

        protected virtual IJsonSerializerProvider GetSerializerProvider(IServiceProvider serviceProvider,Type type)
        {
            foreach (var providerType in Options.Providers.Reverse())
            {
                var provider = serviceProvider.GetRequiredService(providerType) as IJsonSerializerProvider;
                if (provider.CanHandle(type))
                {
                    return provider;
                }
            }

            throw new FrameworkException($"There is no IJsonSerializerProvider that can handle '{type.GetFullNameWithAssemblyName()}'!");
        }
    }
}
