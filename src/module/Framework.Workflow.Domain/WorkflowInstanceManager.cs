using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Domain
{
    public class WorkflowInstanceManager<TWorkflowInstance> : IDisposable where TWorkflowInstance : class
    {
        protected IWorkflowInstanceStore<TWorkflowInstance> Store { get; set; }

        protected virtual CancellationToken CancellationToken { get; set; } = CancellationToken.None;

        protected IWorkflowNodeInstanceStore<TWorkflowInstance> NodeInstanceStore
        {
            get
            {
                return (IWorkflowNodeInstanceStore<TWorkflowInstance>)Store;
            }
        }

        protected IWorkflowInstanceLogStore<TWorkflowInstance> InstanceLogStore
        {
            get
            {
                return (IWorkflowInstanceLogStore<TWorkflowInstance>)Store;
            }
        }

        public WorkflowInstanceManager(IWorkflowInstanceStore<TWorkflowInstance> store)
        {
            Store = store;
        }

        /// <summary>
        ///  根据路由键,查询到对应的待执行的流程实例信息
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="state"></param>FindNodeByIdAsync
        /// <returns></returns>
        public virtual async Task<IEnumerable<WorkflowNodeInstanceInfo>> FindInstancesByRouteKeyAsync(string routeKey, WorkflowNodeInstanceState state)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(routeKey))
            {
                throw new ArgumentNullException(nameof(routeKey));
            }

            return await NodeInstanceStore.FindByRouteKeyAndStateAsync(routeKey, state, CancellationToken);
        }

        /// <summary>
        /// 根据工作流实例Id获取所有的节点实例
        /// </summary>
        /// <param name="workflowInstanceId"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<WorkflowNodeInstanceInfo>> FindByWorkflowInstanceIdAsync(string workflowInstanceId)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(workflowInstanceId))
            {
                throw new ArgumentNullException(nameof(workflowInstanceId));
            }

            return await NodeInstanceStore.FindByWorkflowInstanceIdAsync(workflowInstanceId, CancellationToken);
        }

        public virtual async Task<IEnumerable<WorkflowInstanceLogInfo>> FindLogsByWorkflowInstanceIdAsync(string workflowInstanceId)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(workflowInstanceId))
            {
                throw new ArgumentNullException(nameof(workflowInstanceId));
            }

            return await InstanceLogStore.FindLogsByWorkflowInstanceIdAsync(workflowInstanceId, CancellationToken);
        }

        public async Task UpdateNodeStateAsync(string nodeInstanceId, WorkflowNodeInstanceState state, string description)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(nodeInstanceId))
            {
                throw new ArgumentNullException(nameof(nodeInstanceId));
            }
            await NodeInstanceStore.UpdateNodeStateAsync(nodeInstanceId, state, description, CancellationToken);
        }

        /// <summary>
        /// 可执行的节点实例
        /// </summary>
        /// <param name="routeKey"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<WorkflowNodeInstanceInfo>> CouldExecutedInstancesByRouteKeyAsync(string routeKey, WorkflowNodeInstanceState state)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(routeKey))
            {
                throw new ArgumentNullException(nameof(routeKey));
            }
            var instances=await FindInstancesByRouteKeyAsync(routeKey, state);

            var result = new List<WorkflowNodeInstanceInfo>();
            foreach (var instance in instances)
            {
                //最上级节点,节点处于待执行状态
                if (instance.Prev == null && instance.State == WorkflowNodeInstanceState.Pre)
                {
                    result.Add(instance);
                }

                //上级节点不为空,且上级节点执行成功,当前节点处于待执行状态
                if (instance.Prev != null && instance.Prev.State== WorkflowNodeInstanceState.Succeed && instance.State== WorkflowNodeInstanceState.Pre)
                {
                    result.Add(instance);
                }
            }
            return result;
        }

        public virtual async Task CreateAsync(TWorkflowInstance workflowInstance)
        {
            ThrowIfDisposed();
            if (workflowInstance == null)
            {
                throw new ArgumentNullException(nameof(workflowInstance));
            }
            await Store.CreateAsync(workflowInstance, CancellationToken);
        }

        public virtual async Task CreateNodeInstancesAsync(TWorkflowInstance workflowInstance, IEnumerable<WorkflowNodeInstanceInfo> workflowInstanceInfos)
        {
            ThrowIfDisposed();
            if (workflowInstance == null)
            {
                throw new ArgumentNullException(nameof(workflowInstance));
            }
            if(workflowInstanceInfos==null || workflowInstanceInfos.Count()==0)
            {
                throw new ArgumentNullException(nameof(workflowInstanceInfos));
            }
            await NodeInstanceStore.CreateNodesAsync(workflowInstance, workflowInstanceInfos, CancellationToken);
        }

        public virtual Task<TWorkflowInstance> FindWorkflowInstanceByIdAsync(string nodeInstanceId)
        {
            if (string.IsNullOrEmpty(nodeInstanceId))
                throw new ArgumentNullException(nameof(nodeInstanceId));
            return NodeInstanceStore.FindWorkflowInstanceByIdAsync(nodeInstanceId, CancellationToken);
        }

        public virtual Task CreateInstanceLogAsync(TWorkflowInstance workflowInstance, WorkflowInstanceLogInfo workflowInstanceLogInfo)
        {
            if (workflowInstance==null)
                throw new ArgumentNullException(nameof(workflowInstance));
            if (workflowInstanceLogInfo == null)
                throw new ArgumentNullException(nameof(workflowInstanceLogInfo));
            return InstanceLogStore.CreateLogAsync(workflowInstance, workflowInstanceLogInfo, CancellationToken);
        }

        public virtual async Task<IEnumerable<TWorkflowInstance>> FindByIdListAsync(List<string> ids)
        {
            ThrowIfDisposed();
            if (ids.Count <= 0)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            return await InstanceLogStore.FindByIdListAsync(ids, CancellationToken);
        }

        public virtual async Task UpdateNodeAsync(WorkflowNodeInstanceInfo workflowNodeInstanceInfo)
        {
            ThrowIfDisposed();
            if (workflowNodeInstanceInfo==null)
            {
                throw new ArgumentNullException(nameof(workflowNodeInstanceInfo));
            }
            await NodeInstanceStore.UpdateNodeAsync(workflowNodeInstanceInfo, CancellationToken);
        }

        public virtual async Task<WorkflowNodeInstanceInfo> FindNodeByIdAsync(string id)
        {
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            return await NodeInstanceStore.FindNodeByIdAsync(id, CancellationToken);
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
