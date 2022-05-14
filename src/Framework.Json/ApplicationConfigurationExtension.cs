using Framework.Core.Configurations;
using Framework.Json.Newtonsoft;
using Framework.Json.SystemTextJson;
using Framework.Timing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Framework.Json
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用Json序列化模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseJson(this ApplicationConfiguration configuration)
        {
            configuration.UseTiming()
                         .AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.Container.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<SystemTextJsonSerializerOptions>, SystemTextJsonSerializerOptionsSetup>());

            configuration.Container.Configure<JsonOptions>(options =>
            {
                options.Providers.Add<NewtonsoftJsonSerializerProvider>();
                var jsonOptions = configuration.Container.ExecutePreConfiguredActions<JsonOptions>();
                if (jsonOptions.UseHybridSerializer)
                {
                    options.Providers.Add<SystemTextJsonSerializerProvider>();
                }
            });

            configuration.Container.Configure<NewtonsoftJsonSerializerOptions>(options =>
            {
                options.Converters.Add<JsonIsoDateTimeConverter>();
            });

            return configuration;
        }
    }
}
