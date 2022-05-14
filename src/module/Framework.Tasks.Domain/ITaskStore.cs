using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Domain
{
    public interface ITaskStore<TTask> where TTask:class
    {
        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateAsync(TTask task, CancellationToken cancellationToken);

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task RemoveAsync(TTask task, CancellationToken cancellationToken);

        /// <summary>
        /// 根据id查找任务
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TTask> FindByIdAsync(string taskId, CancellationToken cancellationToken);

        /// <summary>
        /// 获取所有的任务
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TTask>> GetListAsync(CancellationToken cancellationToken);

        /// <summary>
        /// 判断task是否是有效的task
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> IsValidTaskAsync(TTask task,CancellationToken cancellationToken);

        /// <summary>
        /// 获取任务Id
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetTaskIdAsync(TTask task, CancellationToken cancellationToken);

        /// <summary>
        /// 获取任务状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TaskState> GetTaskStateAsync(TTask task, CancellationToken cancellationToken);

    }
}
