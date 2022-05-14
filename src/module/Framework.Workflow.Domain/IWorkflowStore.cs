using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流管理
    /// </summary>
    /// <typeparam name="TWorkflow"></typeparam>
    public interface IWorkflowStore<TWorkflow> :IDisposable
        where TWorkflow:class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<WorkflowResult> CreateAsync(TWorkflow workflow, CancellationToken cancellationToken);

        /// <summary>
        /// 根据Id查找Workflow
        /// </summary>
        /// <param name="workflowId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TWorkflow> FindByIdAsync(string workflowId, CancellationToken cancellationToken);

        /// <summary>
        /// 获取工作流的名称
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetNameAsync(TWorkflow workflow, CancellationToken cancellationToken);
    }
}
