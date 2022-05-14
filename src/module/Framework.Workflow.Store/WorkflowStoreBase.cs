using Framework.Workflow.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Store
{
    /// <summary>
    /// 工作流管理
    /// </summary>
    /// <typeparam name="TWorkflow"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TWorkflowNode"></typeparam>
    public abstract class WorkflowStoreBase<TWorkflow, TWorkflowInstance,TKey, TWorkflowNode, TWorkflowWorkflowInstance> : 
        IWorkflowStore<TWorkflow>,
        IWorkflowNodeStore<TWorkflow>,
        IWorkflowWorkflowInstanceStore<TWorkflow>
        where TWorkflow: Workflow<TKey>,new()
        where TWorkflowInstance: WorkflowInstance<TKey>, new()
        where TWorkflowNode : WorkflowNode<TKey>, new()
        where TWorkflowWorkflowInstance: WorkflowWorkflowInstance<TKey>, new()
        where TKey:IEquatable<TKey>
    {
        public WorkflowStoreBase(WorkflowErrorDescriber workflowErrorDescriber)
        {
            ErrorDescriber = workflowErrorDescriber;
        }

        public WorkflowErrorDescriber ErrorDescriber { get; set; }

        protected virtual TWorkflowNode CreateWorkflowNode(TWorkflow workflow, WorkflowNodeInfo nodeInfo)
        {
            return new TWorkflowNode()
            {
                WorkflowId= workflow.Id,
                Name= nodeInfo.Name,
                Sort= nodeInfo.Sort,
            };
        }

        public abstract Task<WorkflowResult> CreateAsync(TWorkflow workflow, CancellationToken cancellationToken);

        public abstract Task<TWorkflow> FindByIdAsync(string workflowId, CancellationToken cancellationToken);

        public abstract Task<string> GetNameAsync(TWorkflow workflow, CancellationToken cancellationToken);

        public abstract Task AddNodeAsync(TWorkflow workflow, WorkflowNodeInfo node, CancellationToken cancellationToken);

        public abstract Task<IEnumerable<WorkflowNodeInfo>> GetNodesAsync(TWorkflow workflow, CancellationToken cancellationToken);

        public abstract Task AddToWorkflowInstanceAsync(TWorkflow workflow,string workflowInstanceName, CancellationToken cancellationToken);

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

        public abstract Task UpdateStatus(TWorkflow workflow, CancellationToken cancellationToken);
    }
}
