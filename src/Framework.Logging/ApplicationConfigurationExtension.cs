using Framework.Core;
using Framework.Core.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.Logging
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用日志模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseLogging(this ApplicationConfiguration configuration)
        {
            configuration.Container.AddLogging();
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
