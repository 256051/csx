using Microsoft.Extensions.DependencyInjection;

namespace Framework.Workflow.Domain
{
    public static class WorkflowServiceCollectionExtensions
    {
        /// <summary>
        /// 工作流管理
        /// </summary>
        /// <typeparam name="TWorkflow"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static WorkflowBuilder AddWorkflow<TWorkflow>(this IServiceCollection services) 
            where TWorkflow:class
        {
            services.AddScoped<WorkflowManager<TWorkflow>>();
            return new WorkflowBuilder(typeof(TWorkflow), services);
        }
    }
}
