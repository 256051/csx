using Framework.BackgroundJobs.Abstractions;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Framework.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Quartz
{
    public class BackgroundJobQuartzModuleRunner : IModuleRunner, ISingleton
    {
        public void RunAsync(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<BackgroundJobOptions>>().Value;
            if (!options.IsJobExecutionEnabled)
            {
                var quartzOptions = provider.GetRequiredService<IOptions<QuartzOptions>>().Value;
                quartzOptions.StartSchedulerFactory = scheduler => Task.CompletedTask;
            }
        }

        public void StopAsync(IServiceProvider provider)
        {
            
        }
    }
}
