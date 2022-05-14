using Framework.Json.SystemTextJson;
using Framework.Json.SystemTextJson.JsonConverters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;

namespace Framework.AspNetCore.Mvc.Mvc.Json
{
    /// <summary>
    /// 配置SystemTextJson,加入一些自定义序列化器
    /// </summary>
    public class JsonOptionsSetup : IConfigureOptions<JsonOptions>
    {
        protected IServiceProvider ServiceProvider { get; }
        public JsonOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(JsonOptions options)
        {
            options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
            options.JsonSerializerOptions.AllowTrailingCommas = true;
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<DateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<NullableDateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(new StringToEnumFactory());
            options.JsonSerializerOptions.Converters.Add(new StringToBooleanConverter());
            options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
        }
    }
}
