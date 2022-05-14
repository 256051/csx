using Framework.Core.Dependency;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace Framework.Json.SystemTextJson
{
    public class SystemTextJsonSerializerProvider : IJsonSerializerProvider, ITransient
    {
        protected SystemTextJsonSerializerOptions Options { get; }

        protected SystemTextJsonUnsupportedTypeMatcher SystemTextJsonUnsupportedTypeMatcher { get; }

        public SystemTextJsonSerializerProvider(
            IOptions<SystemTextJsonSerializerOptions> options,
            SystemTextJsonUnsupportedTypeMatcher systemTextJsonUnsupportedTypeMatcher)
        {
            SystemTextJsonUnsupportedTypeMatcher = systemTextJsonUnsupportedTypeMatcher;
            Options = options.Value;
        }

        public bool CanHandle(Type type)
        {
            return !SystemTextJsonUnsupportedTypeMatcher.Match(type);
        }

        public string Serialize(object obj, bool camelCase = true, bool indented = false)
        {
            return JsonSerializer.Serialize(obj, CreateJsonSerializerOptions(camelCase, indented));
        }

        public T Deserialize<T>(string jsonString, bool camelCase = true)
        {
            return JsonSerializer.Deserialize<T>(jsonString, CreateJsonSerializerOptions(camelCase));
        }

        public object Deserialize(Type type, string jsonString, bool camelCase = true)
        {
            return JsonSerializer.Deserialize(jsonString, type, CreateJsonSerializerOptions(camelCase));
        }

        protected virtual JsonSerializerOptions CreateJsonSerializerOptions(bool camelCase = true, bool indented = false)
        {
            var settings = new JsonSerializerOptions(Options.JsonSerializerOptions);

            if (camelCase)
            {
                settings.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            }

            if (indented)
            {
                settings.WriteIndented = true;
            }

            return settings;
        }
    }
}
