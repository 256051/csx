using Framework.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.BackgroundJobs.Quartz
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用Quartz后台工作项模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseBackgroundQuartzJobs(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.Container.AddTransient(typeof(QuartzJobExecutionAdapter<>));
            configuration.AddModuleRunner<BackgroundJobQuartzModuleRunner>();
            return configuration;
        }
    }
}
