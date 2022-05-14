using Framework.Tasks.Domain;
using Framework.Tasks.Store;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Memory
{
    public class TaskSessionStore<TTaskSession,TTask,TKey> : TaskSessionStore<TTaskSession, TTask, TKey, TaskSessionTask<TKey>>
        where TTaskSession : TaskSession<TKey>
        where TTask : TaskModel<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public TaskSessionStore(ITaskStore<TTask> taskStore, ILogger<TaskSessionStore<TTaskSession, TTask, TKey>> logger) : base(taskStore, logger) { }
    }

    public  class TaskSessionStore<TTaskSession, TTask, TKey, TTaskSessionTask> : TaskSessionStoreBase<TTaskSession, TTask, TKey, TTaskSessionTask>
    where TTaskSession : TaskSession<TKey>
    where TTaskSessionTask : TaskSessionTask<TKey>,new()
    where TTask : TaskModel<TKey>, new()
    where TKey : IEquatable<TKey>
    {
        private ConcurrentDictionary<TKey, TTaskSession> _taskSessions;
        private ILogger<TaskSessionStore<TTaskSession, TTask, TKey>> _logger;
        private const string _prefix = "task session memory store ";
        private ConcurrentDictionary<TKey, TTaskSessionTask> _taskSessionTasks;

        public TaskSessionStore(ITaskStore<TTask> taskStore, ILogger<TaskSessionStore<TTaskSession, TTask, TKey>> logger)
        {
            TaskStore = taskStore;
            _logger = logger;
            _taskSessions = new ConcurrentDictionary<TKey, TTaskSession>();
            _taskSessionTasks = new ConcurrentDictionary<TKey, TTaskSessionTask>();
        }

        public ITaskStore<TTask> TaskStore { get; private set; }

        public override Task CreateAsync(TTaskSession taskSession, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!_taskSessions.ContainsKey(taskSession.Id))
            {
                if (!_taskSessions.TryAdd(taskSession.Id, taskSession))
                {
                    _logger.LogError($"{_prefix}add task session failed");
                }
            }
            return Task.CompletedTask;
        }

        public override Task<TTaskSession> FindByIdAsync(string taskSessionId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (_taskSessions.TryGetValue(taskSessionId.To<TKey>(), out var _taskSession))
            {
                return Task.FromResult(_taskSession);
            }
            else
            {
                _logger.LogError($"{_prefix}get task session id:{taskSessionId} failed");
            }
            return null;
        }

        public override Task RemoveAsync(TTaskSession taskSession, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!_taskSessions.TryRemove(taskSession.Id, out var _))
            {
                _logger.LogError($"{_prefix} {taskSession} remove failed");
            }
            return Task.CompletedTask;
        }

        public override Task UpdateAsync(TTaskSession taskSession, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var id = taskSession.Id.To<TKey>();
            if (_taskSessions.TryGetValue(id, out var existedSession))
            {
                if (existedSession == null)
                    _logger.LogError($"{_prefix} task session lost for task:{taskSession}");

                if (!_taskSessions.TryUpdate(id, taskSession, existedSession))
                {
                    _logger.LogError($"{_prefix} task session update failed for task session:{taskSession}");
                }
            }
            return Task.CompletedTask;
        }
 
        public override async Task BindTaskAsync(TTaskSession taskSession, string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var task = await TaskStore.FindByIdAsync(taskId, cancellationToken);
            if(task==null)
                _logger.LogError($"{_prefix} can not cant task");
            if (taskSession == null)
                _logger.LogError($"{_prefix} tasksession is null");

            var id = taskId.To<TKey>();
            if (_taskSessionTasks.ContainsKey(id))
                _logger.LogError($"{_prefix} task session mapping existed task id:{taskId}");
            if (!_taskSessionTasks.TryAdd(id, CreateTaskSessionTask(taskSession, task)))
            {
                _logger.LogError($"{_prefix} session bind failed");
            }
        }

        public override async Task UnBindTaskAsync(TTaskSession taskSession, string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var task = await TaskStore.FindByIdAsync(taskId, cancellationToken);
            if (task == null)
                _logger.LogError($"{_prefix} can not cant task");
            if (taskSession == null)
                _logger.LogError($"{_prefix} tasksession is null");
            var id = taskId.To<TKey>();
            if (!_taskSessionTasks.TryRemove(id, out _))
            {
                _logger.LogError($"{_prefix} session  task unbind failed");
            }
        }

        public override Task<TTaskSession> GetSessionAsync(string taskId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(taskId))
                _logger.LogError($"{_prefix} taskId is null");
            if (_taskSessionTasks.TryGetValue(taskId.To<TKey>(), out var taskSessionTask))
            {
                if (taskSessionTask.TaskSessionId != null && !taskSessionTask.TaskSessionId.Equals(default))
                {
                    if (_taskSessions.TryGetValue(taskSessionTask.TaskSessionId, out var taskSession))
                    {
                        return Task.FromResult(taskSession);
                    }
                    else
                    {
                        _logger.LogError($"{_prefix}get task session by task id:{taskId} failed");
                    }
                }
                else {
                    _logger.LogError($"{_prefix}get tasksession id is null");
                }
            }
            else {
                _logger.LogError($"{_prefix}  get tasksessiontask is null");
            }
            return null;
        }
    }
}
