using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流Builder
    /// </summary>
    public class WorkflowBuilder
    {
        public WorkflowBuilder(Type workflowType,IServiceCollection services)
        {
            WorkflowType = workflowType;
            Services = services;
        }

        public IServiceCollection Services { get; private set; }

        /// <summary>
        /// 工作流
        /// </summary>
        public Type WorkflowType { get;private set; }

        /// <summary>
        /// 工作流实例类型
        /// </summary>
        public Type WorkflowInstanceType { get; private set; }

        /// <summary>
        /// 加入工作流实例功能
        /// </summary>
        /// <typeparam name="TWorkflowInstance"></typeparam>
        /// <returns></returns>
        public virtual WorkflowBuilder AddWorkflowInstance<TWorkflowInstance>() where TWorkflowInstance : class
        {
            WorkflowInstanceType = typeof(TWorkflowInstance);
            Services.TryAddScoped<WorkflowInstanceManager<TWorkflowInstance>>();
            return this;
        }
    }
}
