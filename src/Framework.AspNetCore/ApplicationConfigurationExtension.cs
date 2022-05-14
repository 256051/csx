using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Framework.Web.AspNetCore
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启动NetCore模块(包含Api基本功能)
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseAspNetCore(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.Container.AddHttpContextAccessor();
            configuration.Container.AddObjectAccessor(new ObjectAccessor<IApplicationBuilder>());
            return configuration;
        }
    }
}
