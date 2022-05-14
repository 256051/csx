using Framework.Core.Dependency;
using Framework.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.BackgroundWorkers
{
    /// <summary>
    /// 异步周期性工作者基类  基于Timer的循环任务
    /// </summary>
    public abstract class AsyncPeriodicBackgroundWorkerBase : BackgroundWorkerBase
    {
        protected AsyncTimer Timer { get; }

        protected AsyncPeriodicBackgroundWorkerBase(
            AsyncTimer timer,
            IServiceProvider serviceProvider,
            ILazyServiceProvider lazyServiceProvider)
        {
            ServiceProvider = serviceProvider;
            Timer = timer;
            Timer.Elapsed = Timer_Elapsed;
            LazyServiceProvider = lazyServiceProvider;
        }

        public override async Task StartAsync(CancellationToken cancellationToken = default)
        {
            await base.StartAsync(cancellationToken);
            Timer.Start(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken = default)
        {
            Timer.Stop(cancellationToken);
            await base.StopAsync(cancellationToken);
        }

        private async Task Timer_Elapsed(AsyncTimer timer)
        {
            await DoWorkAsync();
        }

        private async Task DoWorkAsync()
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                try
                {
                    await DoWorkAsync(new PeriodicBackgroundWorkerContext(scope.ServiceProvider));
                }
                catch (Exception ex)
                {
                    await scope.ServiceProvider
                        .GetRequiredService<IExceptionNotifier>()
                        .NotifyAsync(new ExceptionNotificationContext(ex));

                    Logger.LogException(ex);
                }
            }
        }

        protected abstract Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext);
    }
}
