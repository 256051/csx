using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Framework.Tasks.Domain
{
    /// <summary>
    /// 任务管理Builder
    /// </summary>
    public class TasksBuilder
    {
        public TasksBuilder(Type tasksType, IServiceCollection services)
        {
            TasksType = tasksType;
            Services = services;
        }

        public IServiceCollection Services { get; private set; }

        /// <summary>
        /// 任务类型
        /// </summary>
        public Type TasksType { get;private set; }

        /// <summary>
        /// 任务会话类型
        /// </summary>
        public Type TaskSessionType { get; private set; }

        /// <summary>
        /// 任务会话管理
        /// </summary>
        /// <typeparam name="TTaskSession"></typeparam>
        /// <returns></returns>
        public virtual TasksBuilder AddTaskSession<TTaskSession>() where TTaskSession : class
        {
            TaskSessionType = typeof(TTaskSession);
            Services.AddSingleton<TaskSessionManager<TTaskSession>>();
            return this;
        }
    }
}
