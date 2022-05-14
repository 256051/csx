using Framework.Core.Configurations;
using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Threading;

namespace Framework.Quartz
{
    public class QuartzModuleRunner : IModuleRunner, ISingleton
    {
        private IScheduler _scheduler;

        public void RunAsync(IServiceProvider provider)
        {
            var options = provider.GetRequiredService<IOptions<QuartzOptions>>().Value;

            _scheduler = provider.GetRequiredService<IScheduler>();

            AsyncHelper.RunSync(() => options.StartSchedulerFactory.Invoke(_scheduler));
        }

        public void StopAsync(IServiceProvider provider)
        {
            if (_scheduler.IsStarted)
            {
                AsyncHelper.RunSync(() => _scheduler.Shutdown());
            }
        }
    }
}
