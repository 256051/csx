using Autofac;
using Autofac.Extensions.DependencyInjection;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Framework.Autofac
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 启用Autofac
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseAutofac(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);

            var containerBuilder=new ContainerBuilder();
            var factory = new FrameworkAutofacServiceProviderFactory(containerBuilder);
            configuration.Container.AddObjectAccessor(containerBuilder);
            configuration.Container.AddSingleton((IServiceProviderFactory<ContainerBuilder>)factory);
            return configuration;
        }
    }
}
