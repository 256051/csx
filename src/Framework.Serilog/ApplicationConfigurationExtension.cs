using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Framework.Serilog
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用Serilog日志模块 配置参考Framework.Serilog.Tests appsetting.json文件注释
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseSerilog(this ApplicationConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration.Container.GetConfiguration())
                .CreateLogger();

            configuration.Container.AddLogging(loggingBuilder =>loggingBuilder.AddSerilog());
            return configuration;
        }
    }
}
