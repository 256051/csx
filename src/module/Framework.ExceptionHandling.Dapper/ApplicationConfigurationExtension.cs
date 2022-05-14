using Framework.Core.Configurations;
using System.Reflection;

namespace Framework.ExceptionHandling.Dapper
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用异常模块 -Dapper持久化
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseExceptionHandlingDapper(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
