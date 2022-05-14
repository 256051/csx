using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Domain
{
    public class TaskManager<TTask> where TTask:class
    {
        protected ITaskStore<TTask> Store { get; set; }

        protected TaskOptions TaskOptions { get; set; }

        protected ITaskStateStore<TTask> StateStore
        {
            get
            {
                return Store as ITaskStateStore<TTask>;
            }
        }

        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        public TaskManager(ITaskStore<TTask> store,IOptions<TaskOptions> options)
        {
            Store = store;
            TaskOptions = options.Value;
        }

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual async Task CreateAsync(TTask task)
        {
            await Store.CreateAsync(task, CancellationToken);
        }

        /// <summary>
        /// 是否可以创建任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual async Task CouldCreateAsync(TTask task)
        {
            await Store.CreateAsync(task, CancellationToken);
        }

        /// <summary>
        /// 获取可以执行的任务 待执行、执行失败
        /// todo 是否需要分一个线程出去单独执行  执行失败的任务
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TTask>> GetCouldExecutedTasksAsync()
        {
            var preTasks=await StateStore.FindTaskByStateAsync(TaskState.Pre, CancellationToken);
            var failedTasks = await StateStore.FindTaskByStateAsync(TaskState.Fail, CancellationToken);
            return preTasks.Union(failedTasks).Distinct();
        }

        /// <summary>
        /// 获取任务Id
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual Task<string> GetTaskIdAsync(TTask task)=> Store.GetTaskIdAsync(task, CancellationToken);


        private static SemaphoreSlim _lock = new SemaphoreSlim(1,1);
        /// <summary>
        /// 运行任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual async Task<bool> TaskRunAsync(TTask task)
        {
            await _lock.WaitAsync();
            var runTasks = await StateStore.FindTaskByStateAsync(TaskState.Executing, CancellationToken);
            //正在运行的任务不能超过配置指定的任务数
            if (runTasks.Count() >= TaskOptions.MaxTaskCount && TaskOptions.MaxTaskCount != 0)
            {
                _lock.Release();
                return false;
            }
                
            var taskId = await Store.GetTaskIdAsync(task, CancellationToken);
            if (string.IsNullOrEmpty(taskId))
            {
                _lock.Release();
                return false;
            }
            var existedTask = await Store.FindByIdAsync(taskId, CancellationToken);
            //任务存在且正在执行,那么不行在执行了
            if (existedTask != null && (await Store.GetTaskStateAsync(existedTask, CancellationToken)) == TaskState.Executing)
            {
                _lock.Release();
                return false;
            }
            await StateStore.SetStateAsync(task, TaskState.Executing, CancellationToken);
            _lock.Release();
            return true;

        }

        /// <summary>
        /// 移除任务
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        public virtual Task RemoveAsync(TTask task)
        {
            return StateStore.RemoveAsync(task, CancellationToken);
        }
    }
}
