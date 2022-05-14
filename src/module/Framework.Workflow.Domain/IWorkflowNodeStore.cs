using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流节点管理
    /// </summary>
    /// <typeparam name="TWorkflow"></typeparam>
    public interface IWorkflowNodeStore<TWorkflow> : IWorkflowStore<TWorkflow> where TWorkflow:class
    {
        /// <summary>
        /// 增加节点信息
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="node"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task AddNodeAsync(TWorkflow workflow, WorkflowNodeInfo node, CancellationToken cancellationToken);

        /// <summary>
        /// 根据工作流信息获取所有的节点
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowNodeInfo>> GetNodesAsync(TWorkflow workflow, CancellationToken cancellationToken);

        /// <summary>
        /// 更新节点信息
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        Task UpdateStatus(TWorkflow workflow, CancellationToken cancellationToken);
    }
}
