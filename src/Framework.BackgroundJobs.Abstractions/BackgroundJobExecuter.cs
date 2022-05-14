using Framework.Core;
using Framework.Core.Dependency;
using Framework.Core.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Abstractions
{
    public class BackgroundJobExecuter : IBackgroundJobExecuter, ITransient
    {
        public ILogger<BackgroundJobExecuter> Logger { get; }

        protected BackgroundJobOptions Options { get; }

        public BackgroundJobExecuter(
            IOptions<BackgroundJobOptions> options,
            ILogger<BackgroundJobExecuter> logger)
        {
            Options = options.Value;
            Logger = logger;
        }

        public virtual async Task ExecuteAsync(JobExecutionContext context)
        {
            var job = context.ServiceProvider.GetService(context.JobType);
            if (job == null)
            {
                throw new FrameworkException("The job type is not registered to DI: " + context.JobType);
            }

            var jobExecuteMethod = context.JobType.GetMethod(nameof(IAsyncBackgroundJob<object>.ExecuteAsync));
            if (jobExecuteMethod == null)
            {
                throw new FrameworkException($"Given job type does not implement {typeof(IAsyncBackgroundJob<>).Name}. " + " The job type was: " + context.JobType);
            }

            try
            {
                await ((Task)jobExecuteMethod.Invoke(job, new[] { context.JobArgs }));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);

                await context.ServiceProvider
                    .GetRequiredService<IExceptionNotifier>()
                    .NotifyAsync(new ExceptionNotificationContext(ex));

                throw new BackgroundJobExecutionException("A background job execution is failed. See inner exception for details.", ex)
                {
                    JobType = context.JobType.AssemblyQualifiedName,
                    JobArgs = context.JobArgs
                };
            }
        }
    }
}
