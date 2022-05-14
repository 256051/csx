using Framework.Core.Dependency;
using Framework.Timing;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Framework.Json.Newtonsoft
{
    /// <summary>
    /// 重写Newtonsoft序列化时间转化器
    /// </summary>
    public class JsonIsoDateTimeConverter : IsoDateTimeConverter, ITransient
    {
        private readonly IClock _clock;
        public JsonIsoDateTimeConverter(IClock clock, IOptions<JsonOptions> jsonOptions)
        {
            _clock = clock;
            if (jsonOptions.Value.DefaultDateTimeFormat != null)
            {
                DateTimeFormat = jsonOptions.Value.DefaultDateTimeFormat;
            }
        }
        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(DateTime) || objectType == typeof(DateTime?))
            {
                return true;
            }

            return false;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var date = base.ReadJson(reader, objectType, existingValue, serializer) as DateTime?;

            if (date.HasValue)
            {
                return _clock.Normalize(date.Value);
            }
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var date = value as DateTime?;
            base.WriteJson(writer, date.HasValue ? _clock.Normalize(date.Value) : value, serializer);
        }
    }
}
