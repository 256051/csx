using Framework.Core.Configurations;
using Framework.Timing;
using System.Reflection;

namespace Framework.DDD.Domain
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用DDD domain模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseDomain(this ApplicationConfiguration configuration)
        {
            configuration.UseTiming();
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
