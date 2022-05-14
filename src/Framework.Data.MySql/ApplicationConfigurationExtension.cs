using Framework.Core;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Framework.Data.MySql
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用MySql模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseMySql(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.Container.Configure<MySqlDbOptions>(configuration.Container.GetConfiguration().GetSection(MySqlDbOptionsProvider.ConfigurationKey));
            return configuration;
        }
    }
}
