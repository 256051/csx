using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.BackgroundJobs.Abstractions
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用后台工作者调度任务模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseBackgroundJobs(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.AddModuleConventionalRegistrar<BackgroundJobsConventionalRegistrar>();
            return configuration;
        }
    }
}
