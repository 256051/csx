using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Domain
{
    /// <summary>
    /// 状态任务相关
    /// </summary>
    /// <typeparam name="TTask"></typeparam>
    public interface ITaskStateStore<TTask>: ITaskStore<TTask> where TTask : class
    {
        /// <summary>
        /// 设置任务状态
        /// </summary>
        /// <param name="task"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SetStateAsync(TTask task, TaskState taskState,CancellationToken cancellationToken);

        /// <summary>
        /// 根据任务状态获取任务
        /// </summary>
        /// <param name="taskState"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TTask>> FindTaskByStateAsync(TaskState taskState, CancellationToken cancellationToken);
    }
}
