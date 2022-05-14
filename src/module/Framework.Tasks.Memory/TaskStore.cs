using Framework.Tasks.Domain;
using Framework.Tasks.Store;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Memory
{
    public class TaskStore<TTask, TKey> : TaskStoreBase<TTask, TKey>
        where TTask : TaskModel<TKey>
        where TKey : IEquatable<TKey>
    {
        private ConcurrentDictionary<TKey, TTask> _tasks;
        private ILogger<TaskStore<TTask, TKey>> _logger;
        private const string _prefix = "task memory store ";

        public TaskStore(ILogger<TaskStore<TTask, TKey>> logger)
        {
            _tasks = new ConcurrentDictionary<TKey, TTask>();
            _logger = logger;
        }

        public override Task CreateAsync(TTask task, CancellationToken cancellationToken)
        {
            if (!_tasks.ContainsKey(task.Id))
            {
                if (!_tasks.TryAdd(task.Id, task))
                {
                    _logger.LogError($"{_prefix}add task failed");
                }
            }
            return Task.CompletedTask;
        }

        public override Task<TTask> FindByIdAsync(string taskId, CancellationToken cancellationToken)
        {
            if (_tasks.TryGetValue(taskId.To<TKey>(), out var _task))
            {
                return Task.FromResult(_task);
            }
            else
            {
                _logger.LogError($"{_prefix}get task id:{taskId} failed");
            }
            return null;
        }

        public override Task<IEnumerable<TTask>> GetListAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(_tasks.Values.AsEnumerable());
        }

        public override Task<bool> IsValidTaskAsync(TTask task, CancellationToken cancellationToken)
        {
            var flag = false;
            if (task.Id == null || task.Id.Equals(default(TKey)))
                _logger.LogError($"{task} id can not be null");
            else if (string.IsNullOrEmpty(task.Name))
                _logger.LogError($"{task} name can not be null");
            else
                flag = true;
            return Task.FromResult(flag);
        }

        public override Task RemoveAsync(TTask task, CancellationToken cancellationToken)
        {
            if(!_tasks.TryRemove(task.Id, out var _))    
            {
                _logger.LogError($"{task} remove failed");
            }
            return Task.CompletedTask;
        }

        public override Task<IEnumerable<TTask>> FindTaskByStateAsync(TaskState taskState, CancellationToken cancellationToken)
        {
            return Task.FromResult(_tasks.Values.Where(task => (int)task.TaskState == (int)taskState));
        }

        public override Task SetStateAsync(TTask task, TaskState taskState, CancellationToken cancellationToken)
        {
            if (_tasks.TryGetValue(task.Id, out var existedTask))
            {
                existedTask.TaskState = taskState;
                if (!_tasks.TryUpdate(task.Id, existedTask, task))
                {
                    _logger.LogError($"{task} update state:{(int)taskState} failed");
                }
            }
            return Task.CompletedTask;
        }
    }
}
