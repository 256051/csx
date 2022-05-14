using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流节点实例仓储
    /// </summary>
    public interface IWorkflowNodeInstanceStore<TWorkflowInstance>: IWorkflowInstanceStore<TWorkflowInstance> where TWorkflowInstance : class
    {
        /// <summary>
        /// 创建流程节点实例
        /// </summary>
        /// <param name="workflowInstance"></param>
        /// <param name="workflowInstanceInfos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateNodesAsync(TWorkflowInstance workflowInstance,IEnumerable<WorkflowNodeInstanceInfo> workflowInstanceInfos, CancellationToken cancellationToken);

        /// <summary>
        /// 根据路由键,查询到对应的待执行的流程实例信息
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="state"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowNodeInstanceInfo>> FindByRouteKeyAndStateAsync(string routeKey, WorkflowNodeInstanceState state, CancellationToken cancellationToken);

        /// <summary>
        /// 获取所有节点实例根据工作流实例Id
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <param name="state"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowNodeInstanceInfo>> FindByWorkflowInstanceIdAsync(string workflowInstanceId,  CancellationToken cancellationToken);

        /// <summary>
        /// 更新节点状态,并给出更新的理由 更新当前节点状态时,级联更新当前节点所在工作流的实例状态,并给出描述
        /// </summary>
        /// <param name="nodeInstanceId"></param>
        /// <param name="state"></param>
        /// <param name="description"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateNodeStateAsync(string nodeInstanceId, WorkflowNodeInstanceState state,string description, CancellationToken cancellationToken);

        /// <summary>
        /// 根据工作流节点实例Id获取工作流实例
        /// </summary>
        /// <param name="nodeInstanceId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TWorkflowInstance> FindWorkflowInstanceByIdAsync(string nodeInstanceId, CancellationToken cancellationToken);

        /// <summary>
        /// 更新节点
        /// </summary>
        /// <param name="workflowNodeInstanceInfo"></param>
        /// <returns></returns>
        Task UpdateNodeAsync(WorkflowNodeInstanceInfo workflowNodeInstanceInfo, CancellationToken cancellationToken);

        /// <summary>
        /// 根据Id查找工作流实例节点信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<WorkflowNodeInstanceInfo> FindNodeByIdAsync(string id, CancellationToken cancellationToken);
    }
}
