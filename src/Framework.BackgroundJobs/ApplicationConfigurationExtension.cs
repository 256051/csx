using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.BackgroundJobs
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用后台工作者调度任务模块-基于Timer简易调度器
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseBackgroundJobsByTimer(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.AddModuleRunner<BackgroundJobsModuleRunner>();
            return configuration;
        }
    }
}
