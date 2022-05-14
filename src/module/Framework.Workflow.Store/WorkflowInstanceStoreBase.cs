using Framework.Timing;
using Framework.Workflow.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Store
{
    public abstract class WorkflowInstanceStoreBase<TWorkflowInstance, TKey, TWorkflowNodeInstance, TWorkflowInstanceLog> : 
        IWorkflowNodeInstanceStore<TWorkflowInstance>,
        IWorkflowInstanceLogStore<TWorkflowInstance>
        where TWorkflowInstance : WorkflowInstance<TKey>,new()
        where TWorkflowNodeInstance: WorkflowNodeInstance<TKey>,new()
        where TWorkflowInstanceLog : WorkflowInstanceLog<TKey>, new()
        where TKey:IEquatable<TKey>
    {
        protected IClock Clock;

        public WorkflowInstanceStoreBase(IClock clock)
        {
            Clock = clock;
        }

        protected virtual TWorkflowInstanceLog CreateWorkflowInstanceLog(TWorkflowInstance workflowInstance, WorkflowInstanceLogInfo logInfo)
        {
            return new TWorkflowInstanceLog()
            {
                WorkflowInstanceId= workflowInstance.Id,
                Content= logInfo.Content,
                CreateTime= logInfo.CreateTime??Clock.Now
            };
        }

        public abstract Task CreateNodesAsync(TWorkflowInstance workflowInstance, IEnumerable<WorkflowNodeInstanceInfo> workflowInstanceInfos, CancellationToken cancellationToken);

        public abstract Task<IEnumerable<WorkflowNodeInstanceInfo>> FindByRouteKeyAndStateAsync(string routeKey, WorkflowNodeInstanceState state, CancellationToken cancellationToken);

        public abstract Task CreateAsync(TWorkflowInstance workflowInstance, CancellationToken cancellationToken);

        public abstract Task UpdateNodeStateAsync(string nodeInstanceId, WorkflowNodeInstanceState state, string description, CancellationToken cancellationToken);

        public abstract Task UpdateStateAsync(string id, WorkflowInstanceState state,  CancellationToken cancellationToken);

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

        public abstract Task CreateLogAsync(TWorkflowInstance workflowInstance, WorkflowInstanceLogInfo logInfo, CancellationToken cancellationToken);

        public abstract Task<TWorkflowInstance> FindWorkflowInstanceByIdAsync(string nodeInstanceId, CancellationToken cancellationToken);

        public abstract Task<IEnumerable<WorkflowNodeInstanceInfo>> FindByWorkflowInstanceIdAsync(string workflowInstanceId, CancellationToken cancellationToken);

        public abstract Task<IEnumerable<WorkflowInstanceLogInfo>> FindLogsByWorkflowInstanceIdAsync(string workflowInstanceId, CancellationToken cancellationToken);

        public abstract Task<IEnumerable<TWorkflowInstance>> FindByIdListAsync(List<string> ids, CancellationToken cancellationToken);

        public abstract Task UpdateNodeAsync(WorkflowNodeInstanceInfo workflowNodeInstanceInfo, CancellationToken cancellationToken);

        public abstract Task<WorkflowNodeInstanceInfo> FindNodeByIdAsync(string id, CancellationToken cancellationToken);
    }
}
