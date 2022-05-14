using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.ExceptionHandling
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用异常模块
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseException(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
