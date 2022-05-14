using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Clients
{
    public interface IProgressClientManager
    {
        /// <summary>
        /// 创建进度任务Hub连接
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <param name="cancellactionToken"></param>
        /// <returns></returns>
        Task CreateAsync(string taskId, string userId, CancellationToken cancellactionToken = default);

        /// <summary>
        /// 查找Hub连接是否存在根据任务Id
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cancellactionToken"></param>
        /// <returns></returns>
        Task<bool> FindByTaskIdAsync(string taskId, CancellationToken cancellactionToken = default);

        /// <summary>
        /// 移除进度任务Hub连接
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cancellactionToken"></param>
        /// <returns></returns>
        Task RemoveAsync(string taskId, CancellationToken cancellactionToken = default);

        /// <summary>
        /// 推送任务进度
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="progress"></param>
        /// <param name="cancellactionToken"></param>
        /// <returns></returns>
        Task PushProgressAsync(string taskId,int progress,CancellationToken cancellactionToken = default);
    }
}
