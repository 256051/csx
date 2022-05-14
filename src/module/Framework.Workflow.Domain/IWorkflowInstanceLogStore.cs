using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流实例日志仓储
    /// </summary>
    public interface IWorkflowInstanceLogStore<TWorkflowInstance>: IWorkflowInstanceStore<TWorkflowInstance> where TWorkflowInstance : class
    {
        /// <summary>
        /// 创建节点运行日志
        /// </summary>
        /// <param name="nodeInstanceId"></param>
        /// <param name="content"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateLogAsync(TWorkflowInstance workflowInstance, WorkflowInstanceLogInfo logInfo, CancellationToken cancellationToken);

        /// <summary>
        /// 根据工作流实例Id获取所有执行日志
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowInstanceLogInfo>> FindLogsByWorkflowInstanceIdAsync(string workflowInstanceId,  CancellationToken cancellationToken);
    }
}
