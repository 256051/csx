using Framework.BackgroundJobs.Abstractions;
using Framework.BackgroundWorkers;
using Framework.Core.Dependency;
using Framework.Timing;
using Framework.Uow;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs
{
    public class BackgroundJobWorker:AsyncPeriodicBackgroundWorkerBase, IBackgroundJobWorker
    {
        protected BackgroundJobOptions JobOptions { get; }

        protected BackgroundJobWorkerOptions WorkerOptions { get; }

        protected IUnitOfWorkManager UnitOfWorkManager { get; }

        public BackgroundJobWorker(
            AsyncTimer timer,
            IOptions<BackgroundJobOptions> jobOptions,
            IOptions<BackgroundJobWorkerOptions> workerOptions,
            IServiceProvider serviceProvider,
            ILazyServiceProvider lazyServiceProvider, 
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                timer,
                serviceProvider,
                lazyServiceProvider)
        {
            WorkerOptions = workerOptions.Value;
            JobOptions = jobOptions.Value;
            Timer.Period = WorkerOptions.JobPollPeriod;
            UnitOfWorkManager = unitOfWorkManager;
        }

        protected override async Task DoWorkAsync(PeriodicBackgroundWorkerContext workerContext)
        {
            var uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions(true));

            var store = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobStore>();

            var waitingJobs = await store.GetWaitingJobsAsync(WorkerOptions.MaxJobFetchCount);

            if (!waitingJobs.Any())
            {
                return;
            }

            var jobExecuter = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobExecuter>();
            var clock = workerContext.ServiceProvider.GetRequiredService<IClock>();
            var serializer = workerContext.ServiceProvider.GetRequiredService<IBackgroundJobSerializer>();

            foreach (var jobInfo in waitingJobs)
            {
                jobInfo.TryCount++;
                jobInfo.LastTryTime = clock.Now;

                try
                {
                    var jobConfiguration = JobOptions.GetJob(jobInfo.JobName);
                    var jobArgs = serializer.Deserialize(jobInfo.JobArgs, jobConfiguration.ArgsType);
                    var context = new JobExecutionContext(workerContext.ServiceProvider, jobConfiguration.JobType, jobArgs);

                    try
                    {
                        await jobExecuter.ExecuteAsync(context);

                        await store.DeleteAsync(jobInfo.Id);
                    }
                    catch (BackgroundJobExecutionException)
                    {
                        var nextTryTime = CalculateNextTryTime(jobInfo, clock);

                        if (nextTryTime.HasValue)
                        {
                            jobInfo.NextTryTime = nextTryTime.Value;
                        }
                        else
                        {
                            jobInfo.IsAbandoned = true;
                        }

                        await TryUpdateAsync(store, jobInfo);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex);
                    jobInfo.IsAbandoned = true;
                    await TryUpdateAsync(store, jobInfo);
                }
            }

            await uow.CompleteAsync();
        }

        /// <summary>
        /// 异常计算下次执行时间
        /// </summary>
        /// <param name="jobInfo"></param>
        /// <param name="clock"></param>
        /// <returns></returns>
        protected virtual DateTime? CalculateNextTryTime(BackgroundJobInfo jobInfo, IClock clock)
        {
            var nextWaitDuration = WorkerOptions.DefaultFirstWaitDuration * (Math.Pow(WorkerOptions.DefaultWaitFactor, jobInfo.TryCount - 1));
            var nextTryDate = jobInfo.LastTryTime?.AddSeconds(nextWaitDuration) ??
                              clock.Now.AddSeconds(nextWaitDuration);
            if (nextTryDate.Subtract(jobInfo.CreationTime).TotalSeconds > WorkerOptions.DefaultTimeout)
            {
                return null;
            }

            return nextTryDate;
        }

        /// <summary>
        /// 更新执行信息
        /// </summary>
        /// <param name="store"></param>
        /// <param name="jobInfo"></param>
        /// <returns></returns>
        protected virtual async Task TryUpdateAsync(IBackgroundJobStore store, BackgroundJobInfo jobInfo)
        {
            try
            {
                await store.UpdateAsync(jobInfo);
            }
            catch (Exception updateEx)
            {
                Logger.LogException(updateEx);
            }
        }
    }
}
