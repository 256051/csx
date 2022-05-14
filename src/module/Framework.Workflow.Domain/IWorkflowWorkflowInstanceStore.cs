using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    public interface IWorkflowWorkflowInstanceStore<TWorkflow>: IWorkflowStore<TWorkflow> where TWorkflow : class
    {
        /// <summary>
        /// 增加工作流和工作流实例的绑定
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="workflowInstanceName">工作流实例名称</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddToWorkflowInstanceAsync(TWorkflow workflow, string workflowInstanceName, CancellationToken cancellationToken);
    }
}
