using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流实例管理
    /// </summary>
    /// <typeparam name="TWorkflow"></typeparam>
    public interface IWorkflowInstanceStore<TWorkflowInstance>  where TWorkflowInstance : class
    {
        /// <summary>
        /// 增加工作流实例
        /// </summary>
        /// <param name="workflowInstance"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateAsync(TWorkflowInstance workflowInstance, CancellationToken cancellationToken);

        /// <summary>
        /// 更新工作流状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateStateAsync(string id, WorkflowInstanceState state, CancellationToken cancellationToken);
        /// <summary>
        /// 根据id获取工作流
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<TWorkflowInstance>> FindByIdListAsync(List<string> ids, CancellationToken cancellationToken);
    }
}
