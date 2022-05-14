using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Framework.RabbitMQ.Client.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.RabbitMQ.Client
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用RabbitMQ客户端模块
        /// </summary>
        /// <returns></returns>
        public static ApplicationConfiguration UseRabbitClient(this ApplicationConfiguration configuration)
        {
            configuration
                .AddModule(Assembly.GetExecutingAssembly().FullName)
                .ConfigModule();
            return configuration;
        }

        /// <summary>
        /// 配置模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        private static ApplicationConfiguration ConfigModule(this ApplicationConfiguration configuration)
        {
            var services = configuration.Container;
            services.Configure<RabbitClientOptions>(services.GetConfiguration().GetSection(RabbitClientOptions.ConfigurationKey));
            return configuration;
        }
    }
}
