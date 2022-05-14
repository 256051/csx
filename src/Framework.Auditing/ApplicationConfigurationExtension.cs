using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.Auditing
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用审计模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseAuditing(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
