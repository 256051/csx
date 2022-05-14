using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Abstractions
{
    /// <summary>
    /// 后台工作项执行者
    /// </summary>
    public interface IBackgroundJobExecuter
    {
        Task ExecuteAsync(JobExecutionContext context);
    }
}
