using Framework.Core.Dependency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime;

namespace Framework.Core.Configurations
{
    /// <summary>
    /// 链式配置
    /// </summary>
    public static class ApplicationConfigurationExtension
    {

        /// <summary>
        /// 启用核心类库,启用ServiceCollection作为核心Di容器,启用配置系统
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseCore(this IServiceCollection services, ConfigurationBuilderOptions options = null)
        {
            ApplicationConfiguration.Current.Container = services;
            ApplicationConfiguration.Current.AddModule(Assembly.GetExecutingAssembly().FullName);
            return ApplicationConfiguration.Current.UseConfiguration(options);
        }

        
        /// <summary>
        /// 初始化配置文件系统 初始化系统配置
        /// </summary>
        /// <returns></returns>
        public static ApplicationConfiguration UseConfiguration(this ApplicationConfiguration application, ConfigurationBuilderOptions options = null) 
        {
            options = options ?? new ConfigurationBuilderOptions();
            if (string.IsNullOrEmpty(options.BasePath))
            {
                options.BasePath = AppDomain.CurrentDomain.BaseDirectory;
            }
            application.Container.AddOptions();
            application.Container.ReplaceConfiguration(
                ConfigurationHelper.BuildConfiguration(options)
             );
            return application;
        }

        /// <summary>
        /// 写入模块 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="application"></param>
        /// <returns></returns>
        public static ApplicationConfiguration AddModule(this ApplicationConfiguration application,string moduleName)
        {
            try
            {
                var assemly = Assembly.Load(moduleName);
                if (!application.Assemblies.Contains(assemly))
                {
                    application.Assemblies.Add(assemly);
                }
            }
            catch (Exception ex)
            {
                throw new FrameworkException($"'{moduleName}' assemny load failed!",ex);
            }
            return application;
        }

        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static IReadOnlyList<Assembly> GetModules(this ApplicationConfiguration application)
        {
            return application.Assemblies;
        }

        /// <summary>
        /// 添加应用程序运行时执行器
        /// </summary>
        /// <param name="application"></param>
        /// <param name="moduleName"></param>
        /// <returns></returns>
        public static ApplicationConfiguration AddModuleRunner<TRunner>(this ApplicationConfiguration application) where TRunner: IModuleRunner
        {
            application.ModuleRunners.Add(typeof(TRunner));
            return application;
        }

        /// <summary>
        /// 注入模块类型注册器
        /// </summary>
        /// <param name="application"></param>
        /// <returns></returns>
        public static ApplicationConfiguration AddModuleConventionalRegistrar<T>(this ApplicationConfiguration application) where T: IConventionalRegistrar,new()
        {
            application.Container.AddModuleConventionalRegistrar(new T());
            return application;
        }

        /// <summary>
        /// 加载所有模块注入到DI中
        /// </summary>
        /// <returns></returns>
        public static ApplicationConfiguration LoadModules(this ApplicationConfiguration configuration)
        {
            var registar = new ModuleRegistar();

            //注入所有托管类型
            configuration.Provider=registar.Register(configuration.Container);

            //加载所有的模块运行器
            configuration.ModuleRunners
                .Select(runnerType => configuration.Provider.GetRequiredService(runnerType) as IModuleRunner).ToList()
                .ForEach(runner => runner.RunAsync(configuration.Provider));

            return configuration;
        }
    }
}
