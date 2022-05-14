using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Domain
{
    /// <summary>
    /// 任务会话关联任务
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    public interface ITaskSessionTaskStore<TTaskSession> : ITaskSessionStore<TTaskSession> where TTaskSession : class
    {
        /// <summary>
        /// 增加任务和任务会话的绑定
        /// </summary>
        /// <param name="taskSession"></param>
        /// <param name="taskId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task BindTaskAsync(TTaskSession taskSession, string taskId, CancellationToken cancellationToken);

        /// <summary>
        /// 任务和任务会话的绑定解除
        /// </summary>
        /// <param name="taskSession"></param>
        /// <param name="taskId"></param>
        /// <param name="cancellationToken"></param>
        Task UnBindTaskAsync(TTaskSession taskSession, string taskId, CancellationToken cancellationToken);

        /// <summary>
        /// 根据任务id获取session
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TTaskSession> GetSessionAsync(string taskId, CancellationToken cancellationToken);
    }
}
