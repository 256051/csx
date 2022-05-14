using Framework.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.BackgroundWorkers.Quartz
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用后台工作者模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseQuartzWorker(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.AddModuleRunner<BackgroundWorkerQuartzModuleRunner>();
            configuration.Container.AddSingleton(typeof(QuartzPeriodicBackgroundWorkerAdapter<>));
            return configuration;
        }
    }
}
