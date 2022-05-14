using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流管理
    /// </summary>
    public class WorkflowManager<TWorkflow> : IDisposable where TWorkflow : class
    {
        protected internal IWorkflowStore<TWorkflow> Store { get; set; }

        /// <summary>
        /// 交给对应的子类模块重写
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        public WorkflowManager(IWorkflowStore<TWorkflow> store)
        {
            Store = store;
        }

        /// <summary>
        /// 工作流和工作流实例关联Store
        /// </summary>
        protected internal IWorkflowWorkflowInstanceStore<TWorkflow> WorkflowWorkflowInstanceStore
        {
            get
            {
                return (IWorkflowWorkflowInstanceStore<TWorkflow>)Store;
            }
        }

        /// <summary>
        /// 工作流节点Store
        /// </summary>
        protected internal IWorkflowNodeStore<TWorkflow> WorkflowNodeStore
        {
            get
            {
                return (IWorkflowNodeStore<TWorkflow>)Store;
            }
        }

        

        /// <summary>
        /// 根据Id获取工作流信息
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        public virtual Task<TWorkflow> FindByIdAsync(string workflowId)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(workflowId))
                throw new ArgumentNullException(nameof(workflowId));
            return Store.FindByIdAsync(workflowId, CancellationToken);
        }

        public virtual async Task AddToWorkflowInstanceAsync(TWorkflow workflow,string workflowInstanceName)
        {
            ThrowIfDisposed();
            if (workflow is null)
                throw new ArgumentNullException(nameof(workflow));
            if (string.IsNullOrEmpty(workflowInstanceName))
                throw new ArgumentNullException(nameof(workflowInstanceName));
            await WorkflowWorkflowInstanceStore.AddToWorkflowInstanceAsync(workflow, workflowInstanceName,CancellationToken);
        }

        /// <summary>
        /// 根据工作流获取其下面所有的节点信息
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<WorkflowNodeInfo>> GetNodesAsync(TWorkflow workflow)
        {
            ThrowIfDisposed();
            return await WorkflowNodeStore.GetNodesAsync(workflow, CancellationToken);
        }

        /// <summary>
        /// 根据工作流Id获取其下面所有的节点信息
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        //public virtual async Task<IEnumerable<WorkflowNodeInfo>> GetNodesByWorkflowIdAsync(string workflowId)
        //{
        //    ThrowIfDisposed();
        //    var workflow = await FindByIdAsync(workflowId);
        //    if (workflow == null)
        //    {
        //        throw new InvalidOperationException("workflow does't existed");
        //    }
        //    return await GetNodesAsync(workflow);
        //}

        /// <summary>
        /// 更新节点信息
        /// </summary>
        /// <param name="workflow"></param>
        /// <returns></returns>
        public virtual async Task UpdateWorkflowNodeStatus(TWorkflow workflow) 
        {
            ThrowIfDisposed();
            await WorkflowNodeStore.UpdateStatus(workflow, CancellationToken);
        }

        protected void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        private bool _disposed;
        public void Dispose()
        {
            _disposed = true;
        }
    }
}
