using Framework.Core;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Framework.Expressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Framework.Excel.Npoi
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用NpoiExcel模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseNpoiExcel(this ApplicationConfiguration configuration, Action<ExcelOptions> action=null)
        {
            configuration.UseExpression();
            Config(ApplicationConfiguration.Current.Container,action);
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }

        /// <summary>
        /// 配置
        /// </summary>
        public static void Config(IServiceCollection services,Action<ExcelOptions> action=null)
        {
            var options = new ExcelOptions();
            if (action != null)
            {
                action?.Invoke(options);
            }
            else {
                var excelOptions=services.GetConfiguration().GetSection(nameof(ExcelOptions));
                options.ConfigFile = excelOptions["ConfigFile"];
            }
           
            services.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            {
                if (string.IsNullOrEmpty(options?.ConfigFile))
                    throw new FrameworkException("excel config file can not be null if you use excel module!");
                builder.AddJsonFile(options.ConfigFile, optional: true, reloadOnChange: true);
            }));
            services.Configure<ExcelReadOptions>(services.GetConfiguration().GetSection(ExcelReadOptionsProvier.ConfigurationKey));
            services.Configure<ExcelWriteOptions>(services.GetConfiguration().GetSection(ExcelWriteOptionsProvier.ConfigurationKey));
        }
    }
}
