using Framework.Core.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using System.Reflection;
using System.Threading;

namespace Framework.Quartz
{
    public static class ApplicationConfigurationExtension
    {
        /// <summary>
        /// 使用Quartz定时器模块
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ApplicationConfiguration UseQuartz(this ApplicationConfiguration configuration)
        {
            configuration.AddModule(Assembly.GetExecutingAssembly().FullName);
            configuration.AddModuleRunner<QuartzModuleRunner>();
            ConfigurationQuartz(configuration);
            return configuration;
        }

        /// <summary>
        /// 配置Quartz
        /// </summary>
        /// <param name="configuration"></param>
        private static void ConfigurationQuartz(ApplicationConfiguration configuration)
        {
            var options = configuration.Container.ExecutePreConfiguredActions<QuartzOptions>();
            configuration.Container.AddQuartz(options.Properties, build =>
            {
                if (options.Properties[StdSchedulerFactory.PropertySchedulerJobFactoryType] == null)
                {
                    build.UseMicrosoftDependencyInjectionScopedJobFactory();
                }

                if (options.Properties[StdSchedulerFactory.PropertySchedulerTypeLoadHelperType] == null)
                {
                    build.UseSimpleTypeLoader();
                }

                if (options.Properties[StdSchedulerFactory.PropertyJobStoreType] == null)
                {
                    build.UseInMemoryStore();
                }

                if (options.Properties[StdSchedulerFactory.PropertyThreadPoolType] == null)
                {
                    build.UseDefaultThreadPool(tp =>
                    {
                        tp.MaxConcurrency = 10;
                    });
                }

                if (options.Properties["quartz.plugin.timeZoneConverter.type"] == null)
                {
                    build.UseTimeZoneConverter();
                }

                options.Configurator?.Invoke(build);
            });
            configuration.Container.AddSingleton(serviceProvider =>
            {
                return AsyncHelper.RunSync(() => serviceProvider.GetRequiredService<ISchedulerFactory>().GetScheduler());
            });
            configuration.Container.Configure<QuartzOptions>(quartzOptions =>
            {
                quartzOptions.Properties = options.Properties;
                quartzOptions.StartDelay = options.StartDelay;
            });
        }
    }
}
