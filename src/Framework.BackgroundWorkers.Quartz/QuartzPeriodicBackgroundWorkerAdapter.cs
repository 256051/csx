using Framework.Core.Dependency;
using Quartz;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.BackgroundWorkers.Quartz
{
    /// <summary>
    /// 适配框架自带工作者,给Quartz托管 只支持异步
    /// </summary>
    /// <typeparam name="TWorker"></typeparam>
    public class QuartzPeriodicBackgroundWorkerAdapter<TWorker> : QuartzBackgroundWorkerBase,
        IQuartzBackgroundWorkerAdapter
        where TWorker : IBackgroundWorker
    {
        private readonly MethodInfo _doWorkAsyncMethod;

        public QuartzPeriodicBackgroundWorkerAdapter() 
        {
            AutoRegister = false;
            _doWorkAsyncMethod = typeof(TWorker).GetMethod("DoWorkAsync", BindingFlags.Instance | BindingFlags.NonPublic);
        }

        public void BuildWorker(IBackgroundWorker worker)
        {
            int? period;
            var workerType = worker.GetType();

            if (worker is AsyncPeriodicBackgroundWorkerBase)
            {
                if (typeof(TWorker) != worker.GetType())
                {
                    throw new ArgumentException($"{nameof(worker)} type is different from the generic type");
                }
                var timer = (AsyncTimer)worker.GetType().GetProperty("Timer", BindingFlags.Instance | BindingFlags.NonPublic)?.GetValue(worker);
                period = timer?.Period;
            }
            else
            {
                return;
            }

            if (period == null)
            {
                return;
            }

            JobDetail = JobBuilder
                .Create<QuartzPeriodicBackgroundWorkerAdapter<TWorker>>()
                .WithIdentity(workerType.FullName)
                .Build();
            Trigger = TriggerBuilder.Create()
                .WithIdentity(workerType.FullName)
                .WithSimpleSchedule(builder => builder.WithInterval(TimeSpan.FromMilliseconds(period.Value)).RepeatForever())
                .Build();
        }

        public override async Task Execute(IJobExecutionContext context)
        {
            var worker = (IBackgroundWorker)ServiceProvider.GetService(typeof(TWorker)) as AsyncPeriodicBackgroundWorkerBase;
            var workerContext = new PeriodicBackgroundWorkerContext(ServiceProvider);

            if (_doWorkAsyncMethod != null)
            {
                await (Task)_doWorkAsyncMethod.Invoke(worker, new object[] { workerContext });
            }
        }
    }
}
