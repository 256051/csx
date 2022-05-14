using Framework.BackgroundJobs.Abstractions;
using System.Threading.Tasks;

namespace Framework.BackgroundJobs
{
    /// <summary>
    /// 异步后台工作项
    /// </summary>
    /// <typeparam name="TArgs"></typeparam>
    public abstract class AsyncBackgroundJob<TArgs> : IAsyncBackgroundJob<TArgs>
    {
        public abstract Task ExecuteAsync(TArgs args);
    }
}
