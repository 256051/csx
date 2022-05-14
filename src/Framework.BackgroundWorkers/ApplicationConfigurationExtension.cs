using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.BackgroundWorkers
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用后台工作者模块 -通过Timer
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseBackgroundWorker(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.AddModuleRunner<BackgroundWorkerModuleRunner>();
            return configuration;
        }
    }
}
