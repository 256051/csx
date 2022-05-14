using Microsoft.Extensions.DependencyInjection;

namespace Framework.Tasks.Domain
{
    public static class TasksServiceCollectionExtensions
    {
        /// <summary>
        /// 集成任务管理任务管理模块
        /// </summary>
        /// <typeparam name="TTask"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static TasksBuilder AddTasks<TTask>(this IServiceCollection services) 
            where TTask : class
        {
            services.AddSingleton<TaskManager<TTask>>();
            return new TasksBuilder(typeof(TTask), services);
        }
    }
}
