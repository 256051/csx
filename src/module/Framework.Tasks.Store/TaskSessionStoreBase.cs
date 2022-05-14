using Framework.Tasks.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Store
{
    public abstract class TaskSessionStoreBase<TTaskSession, TTask, TKey, TTaskSessionTask> :
        ITaskSessionStore<TTaskSession>,
        ITaskSessionTaskStore<TTaskSession>
        where TTaskSession: TaskSession<TKey>
        where TTaskSessionTask: TaskSessionTask<TKey>,new()
        where TTask : TaskModel<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        protected virtual TTaskSessionTask CreateTaskSessionTask(TTaskSession taskSession, TTask task)
        {
            return new TTaskSessionTask()
            {
                TaskId = task.Id,
                TaskSessionId = taskSession.Id
            };
        }
        public abstract Task BindTaskAsync(TTaskSession taskSession, string taskId, CancellationToken cancellationToken);
        public abstract Task CreateAsync(TTaskSession taskSession, CancellationToken cancellationToken);
        public abstract Task<TTaskSession> FindByIdAsync(string taskSessionId, CancellationToken cancellationToken);
        public abstract Task RemoveAsync(TTaskSession taskSession, CancellationToken cancellationToken);
        public abstract Task UpdateAsync(TTaskSession taskSession, CancellationToken cancellationToken);
        public abstract Task<TTaskSession> GetSessionAsync(string taskId, CancellationToken cancellationToken);
        public Task<string> GetIdAsync(TTaskSession taskSession, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (taskSession.Id == null)
                throw new ArgumentNullException(nameof(taskSession.Id));
            return Task.FromResult(taskSession.Id.ToString());
        }

        public abstract Task UnBindTaskAsync(TTaskSession taskSession, string taskId, CancellationToken cancellationToken);
    }
}
