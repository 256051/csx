using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.Timing
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用时间模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseTiming(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
