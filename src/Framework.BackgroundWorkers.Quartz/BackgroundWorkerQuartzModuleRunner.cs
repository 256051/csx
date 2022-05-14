using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Framework.Quartz;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.BackgroundWorkers.Quartz
{
    public class BackgroundWorkerQuartzModuleRunner : IModuleRunner, ISingleton
    {
        public void RunAsync(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<BackgroundWorkerOptions>>().Value;
            if (!options.IsEnabled)
            {
                var quartzOptions = provider.GetRequiredService<IOptions<QuartzOptions>>().Value;
                quartzOptions.StartSchedulerFactory = _ => Task.CompletedTask;
            }

            var quartzBackgroundWorkerOptions = provider.GetRequiredService<IOptions<BackgroundWorkerQuartzOptions>>().Value;
            if (quartzBackgroundWorkerOptions.IsAutoRegisterEnabled)
            {
                var backgroundWorkerManager = provider.GetRequiredService<IBackgroundWorkerManager>();
                var works = provider.GetServices<IQuartzBackgroundWorker>().Where(x => x.AutoRegister);

                foreach (var work in works)
                {
                    backgroundWorkerManager.Add(work);
                }
            }
        }

        public void StopAsync(IServiceProvider provider)
        {
            
        }
    }
}
