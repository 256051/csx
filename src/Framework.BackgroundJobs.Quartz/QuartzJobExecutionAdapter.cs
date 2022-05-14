using Framework.BackgroundJobs.Abstractions;
using Framework.Core.Exceptions;
using Framework.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Quartz
{
    public class QuartzJobExecutionAdapter<TArgs> : IJob
    {
        public ILogger<QuartzJobExecutionAdapter<TArgs>> Logger { get; set; }
        protected BackgroundJobOptions Options { get; }
        protected BackgroundJobQuartzOptions BackgroundJobQuartzOptions { get; }
        protected IServiceProvider ServiceProvider { get; }
        protected IBackgroundJobExecuter JobExecuter { get; }
        protected IJsonSerializer JsonSerializer { get; }

        public QuartzJobExecutionAdapter(
            IOptions<BackgroundJobOptions> options,
            IOptions<BackgroundJobQuartzOptions> backgroundJobQuartzOptions,
            IBackgroundJobExecuter jobExecuter,
            IServiceProvider serviceProvider,
            IJsonSerializer jsonSerializer,
            ILogger<QuartzJobExecutionAdapter<TArgs>> logger)
        {
            JobExecuter = jobExecuter;
            ServiceProvider = serviceProvider;
            JsonSerializer = jsonSerializer;
            Options = options.Value;
            BackgroundJobQuartzOptions = backgroundJobQuartzOptions.Value;
            Logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            using (var scope = ServiceProvider.CreateScope())
            {
                var args = JsonSerializer.Deserialize<TArgs>(context.JobDetail.JobDataMap.GetString(nameof(TArgs)));
                var jobType = Options.GetJob(typeof(TArgs)).JobType;
                var jobContext = new JobExecutionContext(scope.ServiceProvider, jobType, args);
                try
                {
                    await JobExecuter.ExecuteAsync(jobContext);
                }
                catch (Exception exception)
                {
                    var jobExecutionException = new JobExecutionException(exception);

                    var retryIndex = context.JobDetail.JobDataMap.GetString(QuartzBackgroundJobManager.JobDataPrefix + QuartzBackgroundJobManager.RetryIndex).To<int>();
                    retryIndex++;
                    context.JobDetail.JobDataMap.Put(QuartzBackgroundJobManager.JobDataPrefix + QuartzBackgroundJobManager.RetryIndex, retryIndex.ToString());

                    await BackgroundJobQuartzOptions.RetryStrategy.Invoke(retryIndex, context, jobExecutionException);

                    await ServiceProvider.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(jobExecutionException));

                    throw jobExecutionException;
                }
            }
        }
    }
}
