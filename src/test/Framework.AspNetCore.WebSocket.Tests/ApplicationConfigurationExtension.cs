using Framework.AspNetCore.WebSocket.Messages;
using Framework.AspNetCore.WebSocket.Tests.Entities;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WPSM.Data.Mq.Domain;
using WPSM.Data.Mq.Local.Store;

namespace Framework.AspNetCore.WebSocket.Tests
{
    public static class ApplicationConfigurationExtension
    {
        public static ApplicationConfiguration UseApplication(this ApplicationConfiguration configuration)
        {
            //加载所有模块
            configuration
                .AddMessageProviders()
                .UseServiceModules()
                .AddModule(Assembly.GetExecutingAssembly().FullName);

            return configuration;
        }

        /// <summary>
        /// 添加信息提供者
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration AddMessageProviders(this ApplicationConfiguration configuration)
        {
            configuration.Container.AddSingleton(new MessageProvider(Consts.Heart));
            configuration.Container.AddSingleton(new MessageProvider(Consts.Breath));
            return configuration;
        }

        /// <summary>
        /// 加载业务模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseServiceModules(this ApplicationConfiguration configuration)
        {
            configuration
                .UseWPSMLocalMq();
            return configuration;
        }

        /// <summary>
        /// 加载本地生命体征Mq模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseWPSMLocalMq(this ApplicationConfiguration configuration)
        {
            configuration.Container.Configure<LocalOptions>(configuration.Container.GetConfiguration().GetSection(LocalOptionsProvider.ConfigurationKey));
            
            configuration.Container
              .AddWPSMDataMQ<WPSMDataMQLocalInfo>()
              .AddWSPMDataMqLocalStore();
            return configuration;
        }
    }
}
