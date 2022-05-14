using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Domain
{
    /// <summary>
    /// 任务会话
    /// </summary>
    /// <typeparam name="TTaskSession"></typeparam>
    public interface ITaskSessionStore<TTaskSession> where TTaskSession : class
    {
        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="taskSession"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateAsync(TTaskSession taskSession,CancellationToken cancellationToken);

        /// <summary>
        /// 更新会话
        /// </summary>
        /// <returns></returns>
        Task UpdateAsync(TTaskSession taskSession, CancellationToken cancellationToken);

        /// <summary>
        /// 获取会话
        /// </summary>
        /// <param name="taskSessionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TTaskSession> FindByIdAsync(string taskSessionId, CancellationToken cancellationToken);

        /// <summary>
        /// 获取传入session的Id
        /// </summary>
        /// <param name="taskSession"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetIdAsync(TTaskSession taskSession, CancellationToken cancellationToken);

        /// <summary>
        /// 移除会话
        /// </summary>
        /// <param name="taskSession"></param>
        /// <returns></returns>
        Task RemoveAsync(TTaskSession taskSession, CancellationToken cancellationToken);
    }
}
