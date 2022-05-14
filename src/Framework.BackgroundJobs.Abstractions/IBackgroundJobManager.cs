using System.Threading.Tasks;

namespace Framework.BackgroundJobs.Abstractions
{
    /// <summary>
    /// 后台工作项管理类
    /// </summary>
    public interface IBackgroundJobManager
    {
        /// <summary>
        /// 写入任务 支持Corn表达式
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="retryCount"></param>
        /// <param name="retryIntervalMillisecond"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        Task<string> EnqueueAsync<TArgs>(TArgs args, BackgroundJobPriority priority = BackgroundJobPriority.Normal);

        /// <summary>
        /// 停止任务
        /// </summary>
        /// <typeparam name="TArgs"></typeparam>
        /// <param name="args"></param>
        /// <param name="retryCount"></param>
        /// <param name="retryIntervalMillisecond"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        Task DequeueAsync(string jobKey);
    }
}
