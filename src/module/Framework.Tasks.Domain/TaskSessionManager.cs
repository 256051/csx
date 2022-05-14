using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Tasks.Domain
{
    /// <summary>
    /// 任务会话管理
    /// </summary>
    /// <typeparam name="TTaskSession"></typeparam>
    public class TaskSessionManager<TTaskSession> where TTaskSession:class
    {
        public ITaskSessionStore<TTaskSession> Store { get; set; }

        public ITaskSessionTaskStore<TTaskSession> TaskSessionTaskStore 
        {
            get
            {
                return Store as ITaskSessionTaskStore<TTaskSession>;
            }
        }

        public TaskSessionManager(ITaskSessionStore<TTaskSession> store)
        {
            Store = store;
        }

        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// 创建会话
        /// </summary>
        /// <param name="taskSession"></param>
        /// <returns></returns>
        public Task CreateAsync(TTaskSession taskSession)
        {
            if (taskSession == null)
                throw new ArgumentNullException(nameof(taskSession));
            return Store.CreateAsync(taskSession, CancellationToken);
        }

        /// <summary>
        /// 创建会话并绑定Task
        /// </summary>
        /// <param name="taskSession"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public async Task CreateAsync(TTaskSession taskSession,string taskId)
        {
            if (taskSession == null)
                throw new ArgumentNullException(nameof(taskSession));
            await Store.CreateAsync(taskSession, CancellationToken);
            await BindTaskAsync(taskSession, taskId);
        }

        /// <summary>
        /// 更新会话
        /// </summary>
        /// <param name="taskSession"></param>
        /// <returns></returns>
        public async Task UpdateAsync(TTaskSession taskSession)
        {
            if (taskSession == null)
                throw new ArgumentNullException(nameof(taskSession));
            var id = await Store.GetIdAsync(taskSession, CancellationToken);
            var session= await Store.FindByIdAsync(id, CancellationToken);
            if (session == null)
                throw new Exception("task session not found update failed");
            await Store.UpdateAsync(session, CancellationToken);
        }

        /// <summary>
        /// 移除会话
        /// </summary>
        /// <param name="taskSession"></param>
        /// <returns></returns>
        public async Task RemoveAsync(TTaskSession taskSession)
        {
            if (taskSession == null)
                throw new ArgumentNullException(nameof(taskSession));
            var id = await Store.GetIdAsync(taskSession, CancellationToken);
            var session = await Store.FindByIdAsync(id, CancellationToken);
            if (session == null)
                throw new Exception("task session not found remove failed");
            await Store.RemoveAsync(session, CancellationToken);
        }

        /// <summary>
        /// 根据任务Id获取会话
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task<TTaskSession> GetSessionAsync(string taskId)
        { 
            if(string.IsNullOrEmpty(taskId))
                throw new ArgumentNullException(nameof(taskId));
            return TaskSessionTaskStore.GetSessionAsync(taskId, CancellationToken);
        }

        /// <summary>
        /// 绑定会话和任务
        /// </summary>
        /// <param name="taskSession"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task BindTaskAsync(TTaskSession taskSession, string taskId)
        {
            if (taskSession == null)
                throw new ArgumentNullException(nameof(taskSession));
            if (string.IsNullOrEmpty(taskId))
                throw new ArgumentNullException(nameof(taskId));
            return TaskSessionTaskStore.BindTaskAsync(taskSession, taskId, CancellationToken);
        }

        /// <summary>
        /// 解绑会话和任务
        /// </summary>
        /// <param name="taskSession"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Task UnBindTaskAsync(TTaskSession taskSession, string taskId)
        {
            if (taskSession == null)
                throw new ArgumentNullException(nameof(taskSession));
            if (string.IsNullOrEmpty(taskId))
                throw new ArgumentNullException(nameof(taskId));
            return TaskSessionTaskStore.UnBindTaskAsync(taskSession, taskId, CancellationToken);
        }
    }
}
