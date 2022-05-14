using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Framework.Json.SystemTextJson.JsonConverters
{
    public class StringToEnumFactory : JsonConverterFactory
    {
        private readonly JsonNamingPolicy _namingPolicy;
        private readonly bool _allowIntegerValues;

        public StringToEnumFactory() : this(namingPolicy: null, allowIntegerValues: true)
        {

        }

        public StringToEnumFactory(JsonNamingPolicy namingPolicy, bool allowIntegerValues)
        {
            _namingPolicy = namingPolicy;
            _allowIntegerValues = allowIntegerValues;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsEnum;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return (JsonConverter)Activator.CreateInstance(
               typeof(StringToEnumConverter<>).MakeGenericType(typeToConvert),
               BindingFlags.Instance | BindingFlags.Public,
               binder: null,
               new object[] { _namingPolicy, _allowIntegerValues },
               culture: null);
        }
    }
}
