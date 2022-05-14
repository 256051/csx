using Framework.Core.Configurations;
using Framework.Security.Users;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.Security
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用安全模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseSecurity(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            return configuration;
        }
    }
}
