using Quartz;
using System;
using System.Threading.Tasks;

namespace Framework.BackgroundWorkers.Quartz
{
    public interface IQuartzBackgroundWorker : IBackgroundWorker, IJob
    {
        ITrigger Trigger { get; set; }

        IJobDetail JobDetail { get; set; }

        /// <summary>
        /// 标识是否经过适配器转换
        /// </summary>
        bool AutoRegister { get; set; }

        Func<IScheduler, Task> ScheduleJob { get; set; }
    }
}
