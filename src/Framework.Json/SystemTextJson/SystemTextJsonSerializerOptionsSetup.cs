using Framework.Json.SystemTextJson.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Framework.Json.SystemTextJson
{
    /// <summary>
    /// SystemTextJson配置,添加自定义序列化器
    /// </summary>
    public class SystemTextJsonSerializerOptionsSetup : IConfigureOptions<SystemTextJsonSerializerOptions>
    {
        protected IServiceProvider ServiceProvider { get; }
        public SystemTextJsonSerializerOptionsSetup(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public void Configure(SystemTextJsonSerializerOptions options)
        {
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<DateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(ServiceProvider.GetRequiredService<NullableDateTimeConverter>());
            options.JsonSerializerOptions.Converters.Add(new StringToEnumFactory());
            options.JsonSerializerOptions.Converters.Add(new StringToBooleanConverter());
            options.JsonSerializerOptions.Converters.Add(new ObjectToInferredTypesConverter());
        }
    }
}
