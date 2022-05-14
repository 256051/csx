using Framework.Tasks.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Store
{
    public abstract class TaskStoreBase<TTask,TKey> : 
        ITaskStore<TTask>,
        ITaskStateStore<TTask>
        where TTask : TaskModel<TKey>
        where TKey:IEquatable<TKey>
    {
        public abstract Task CreateAsync(TTask task, CancellationToken cancellationToken);
        public abstract Task<TTask> FindByIdAsync(string taskId, CancellationToken cancellationToken);
        public abstract Task<IEnumerable<TTask>> FindTaskByStateAsync(TaskState taskState, CancellationToken cancellationToken);
        public abstract Task<IEnumerable<TTask>> GetListAsync(CancellationToken cancellationToken);

        public Task<string> GetTaskIdAsync(TTask task, CancellationToken cancellationToken)
        {
            return Task.FromResult(task.Id.ToString());
        }

        public Task<TaskState> GetTaskStateAsync(TTask task, CancellationToken cancellationToken)
        {
            return Task.FromResult(task.TaskState);
        }

        public abstract Task<bool> IsValidTaskAsync(TTask task, CancellationToken cancellationToken);
        public abstract Task RemoveAsync(TTask task, CancellationToken cancellationToken);
        public abstract Task SetStateAsync(TTask task, TaskState taskState, CancellationToken cancellationToken);
    }
}
