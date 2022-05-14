using Framework.BackgroundJobs.Abstractions;
using Framework.Core.Dependency;
using Framework.Json;
using Framework.Timing;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Quartz
{
    public class QuartzBackgroundJobManager : IBackgroundJobManager, ITransient,IReplace
    {
        public const string JobDataPrefix = " QuartzBackgroundJob";
        public const string RetryIndex = "RetryIndex";

        protected IScheduler Scheduler { get; }

        protected BackgroundJobQuartzOptions Options { get; }

        protected BackgroundJobOptions JobOptions { get; }

        protected IClock Clock { get; }

        protected IJsonSerializer JsonSerializer { get; }

        public QuartzBackgroundJobManager(
            IScheduler scheduler,
            IOptions<BackgroundJobQuartzOptions> options,
            IJsonSerializer jsonSerializer,
            IOptions<BackgroundJobOptions> jobOptions,
            IClock clock)
        {
            Scheduler = scheduler;
            JsonSerializer = jsonSerializer;
            Options = options.Value;
            JobOptions = jobOptions.Value;
            Clock = clock;
        }

        public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal)
        {
            return await ReEnqueueAsync(args, Options.RetryCount, Options.RetryIntervalMillisecond, priority);
        }

        public virtual async Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal,
            TimeSpan? delay = null)
        {
            return await ReEnqueueAsync(args, Options.RetryCount, Options.RetryIntervalMillisecond, priority, delay);
        }

        public virtual async Task<string> ReEnqueueAsync<TArgs>(TArgs args, int retryCount, int retryIntervalMillisecond,
            BackgroundJobPriority priority = BackgroundJobPriority.Normal, TimeSpan? delay = null)
        {
            var jobDataMap = new JobDataMap
            {
                {nameof(TArgs), JsonSerializer.Serialize(args)},
                {JobDataPrefix+ nameof(Options.RetryCount), retryCount.ToString()},
                {JobDataPrefix+ nameof(Options.RetryIntervalMillisecond), retryIntervalMillisecond.ToString()},
                {JobDataPrefix+ RetryIndex, "0"}
            };

            var jobDetail = JobBuilder.Create<QuartzJobExecutionAdapter<TArgs>>().RequestRecovery().SetJobData(jobDataMap).Build();
            var trigger = !delay.HasValue ? TriggerBuilder.Create().StartNow().Build() : TriggerBuilder.Create().StartAt(new DateTimeOffset(DateTime.Now.Add(delay.Value))).Build();
            await Scheduler.ScheduleJob(jobDetail, trigger);
            return jobDetail.Key.ToString();
        }

        public virtual async Task<string> ReEnqueueAsync<TArgs>(TArgs args, int retryCount, int retryIntervalMillisecond, BackgroundJobPriority priority = BackgroundJobPriority.Normal)
        {
            var jobDataMap = new JobDataMap
            {
                {nameof(TArgs), JsonSerializer.Serialize(args)},
                {JobDataPrefix+ nameof(Options.RetryCount), retryCount.ToString()},
                {JobDataPrefix+ nameof(Options.RetryIntervalMillisecond), retryIntervalMillisecond.ToString()},
                {JobDataPrefix+ RetryIndex, "0"}
            };
            var jobType = JobOptions.GetJob(typeof(TArgs)).JobType;
            var jobConfiguration = jobType.GetCustomAttribute<QuartzJobAttribute>(true) ?? QuartzJobAttribute.Default;

            var unionKey = JsonSerializer.Serialize(args).GetHashCode().ToString();
            var jobKey = new JobKey(unionKey);
            var jobDetail = JobBuilder.Create<QuartzJobExecutionAdapter<TArgs>>()
                .WithIdentity(jobKey)
                .WithDescription(jobConfiguration.Description)
                .StoreDurably(false)
                .SetJobData(jobDataMap).Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity(new TriggerKey(unionKey))
                .StartAt(DateBuilder.EvenSecondDate(Clock.Now.AddSeconds(jobConfiguration.Delay.Value)))
                .WithDescription(jobConfiguration.TriggerDescription??$"the trigger for type '{jobType.FullName}'")
                .WithCronSchedule(jobConfiguration.CornExpression)
                .ForJob(jobKey)
                .Build();

            await Scheduler.ScheduleJob(jobDetail, trigger);
            return unionKey;
        }

        //todo ?直接删除是不是不太妥
        public Task DequeueAsync(string jobKey) => Scheduler.DeleteJob(new JobKey(jobKey));
    }
}
