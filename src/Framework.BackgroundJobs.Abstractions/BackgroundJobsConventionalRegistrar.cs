using Framework.Core.Dependency;
using Framework.Core.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.BackgroundJobs.Abstractions
{
    /// <summary>
    /// 注册项目中所有的工作项到DI容器中
    /// </summary>
    public class BackgroundJobsConventionalRegistrar : IConventionalRegistrar
    {
        public void AddType(IServiceCollection services, Type type)
        {
            if (ReflectionHelper.IsAssignableToGenericType(type, typeof(IAsyncBackgroundJob<>)))
            {
                services.Configure<BackgroundJobOptions>(options =>
                {
                    options.AddJob(type);
                });
            }
        }
    }
}
