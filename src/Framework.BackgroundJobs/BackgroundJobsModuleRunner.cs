using Framework.BackgroundJobs.Abstractions;
using Framework.BackgroundWorkers;
using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Framework.BackgroundJobs
{
    public class BackgroundJobsModuleRunner : IModuleRunner, ISingleton
    {
        public void RunAsync(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<BackgroundJobOptions>>().Value;
            if (options.IsJobExecutionEnabled)
            {
                provider
                    .GetRequiredService<IBackgroundWorkerManager>()
                    .Add(
                        provider
                            .GetRequiredService<IBackgroundJobWorker>()
                    );
            }
        }

        public void StopAsync(IServiceProvider provider)
        {

        }
    }
}
