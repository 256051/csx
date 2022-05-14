using Dapper;
using Dapper.Contrib.Extensions;
using Framework.Core.Data;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Shared;
using Framework.Workflow.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Workflow.Dapper
{
    public class WorkflowStore<TWorkflow, TWorkflowInstance, TKey> : WorkflowStore<TWorkflow, TWorkflowInstance, TKey, WorkflowNode<TKey>, WorkflowWorkflowInstance<TKey>>
        where TWorkflow : Workflow<TKey>, new()
        where TWorkflowInstance : WorkflowInstance<TKey>, new()
        where TKey : IEquatable<TKey>
    { 
        public WorkflowStore(IDbProvider dbProvider,WorkflowErrorDescriber workflowErrorDescriber=null) :base(dbProvider,workflowErrorDescriber ?? new WorkflowErrorDescriber())
        { 
            
        }
    }

    public class WorkflowStore<TWorkflow, TWorkflowInstance, TKey, TWorkflowNode, TWorkflowWorkflowInstance> : WorkflowStoreBase<TWorkflow, TWorkflowInstance, TKey, TWorkflowNode, TWorkflowWorkflowInstance>
        where TWorkflow : Workflow<TKey>, new()
        where TWorkflowNode : WorkflowNode<TKey>, new()
        where TWorkflowInstance : WorkflowInstance<TKey>, new()
        where TWorkflowWorkflowInstance : WorkflowWorkflowInstance<TKey>, new()
        where TKey:IEquatable<TKey>
    {

        protected IDbProvider DbProvider;

        public WorkflowStore(IDbProvider dbProvider,WorkflowErrorDescriber workflowErrorDescriber=null) :base(workflowErrorDescriber)
        {
            DbProvider = dbProvider;
        }

        public override Task AddNodeAsync(TWorkflow workflow, WorkflowNodeInfo nodeInfo, CancellationToken cancellationToken = default)
        {
            var node = CreateWorkflowNode(workflow,nodeInfo);
            return null;
        }

        public override async Task AddToWorkflowInstanceAsync(TWorkflow workflow, string workflowInstanceName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (workflow == null)
            {
                throw new ArgumentNullException(nameof(workflow));
            }
            if (string.IsNullOrWhiteSpace(workflowInstanceName))
            {
                throw new ArgumentException(workflowInstanceName);
            }
            var workflowInstance = await FindWorkFlowInstanceAsync(workflowInstanceName, cancellationToken);
            if (workflowInstance == null)
            {
                throw new InvalidOperationException($"the workflow instance named {workflowInstanceName} not found");
            }
            var workflowWorkflowInstance=CreateWorkflowWorkflowInstance(workflow, workflowInstance);
            var connection = await DbProvider.GetConnectionAsync();
            await connection.InsertAsync(workflowWorkflowInstance, await DbProvider.GetTransactionAsync());
        }

        /// <summary>
        /// Dapper 插入时主键不支持实体跟踪,插入时必须给定指定类型 类型中不能包含泛型属性
        /// </summary>
        /// <param name="workflow"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        protected virtual ApplicationWorkflowWorkflowInstance CreateWorkflowWorkflowInstance(TWorkflow workflow, TWorkflowInstance instance)
        {
            return new ApplicationWorkflowWorkflowInstance()
            {
                WorkflowId = workflow.Id.ToString(),
                WorkflowInstanceId = instance.Id.ToString()
            };
        }

        protected async Task<TWorkflowInstance> FindWorkFlowInstanceAsync(string workflowName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<TWorkflowInstance>("select * from ServiceWorkflowInstances where Name=@Name", new { Name= workflowName }, await DbProvider.GetTransactionAsync());
        }

        public override Task<WorkflowResult> CreateAsync(TWorkflow workflow, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async override Task<TWorkflow> FindByIdAsync(string workflowId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(workflowId))
                throw new ArgumentNullException(nameof(workflowId));
            var connection = await DbProvider.GetConnectionAsync();
            var result= await connection.GetAsync<TWorkflow>(workflowId, await DbProvider.GetTransactionAsync());
            return result;
        }

        public override Task<string> GetNameAsync(TWorkflow workflow, CancellationToken cancellationToken)
        {
            return Task.FromResult(workflow.Name);
        }

        public override async Task<IEnumerable<WorkflowNodeInfo>> GetNodesAsync(TWorkflow workflow, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (workflow == null || workflow.Id.Equals(default(TKey)))
            {
                throw new InvalidOperationException("workflow can't be null");
            }
            var nodes = await (await DbProvider.GetConnectionAsync()).QueryAsync<ApplicationWorkflowNode>($"select * from ServiceWorkflowNodes where WorkflowId=@WorkflowId order by Sort asc", new { WorkflowId = workflow.Id.ToString() }, await DbProvider.GetTransactionAsync());
            return nodes.Select(node=>new WorkflowNodeInfo() { Id= node.Id,Name = node.Name,Sort= node.Sort});
        }

        public override async Task UpdateStatus(TWorkflow workflow, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (workflow == null || workflow.Id.Equals(default(TKey)))
            {
                throw new InvalidOperationException("workflow can't be null");
            }
            await (await DbProvider.GetConnectionAsync()).ExecuteAsync("update serviceworkflownodeinstances set state = 0 where WorkflowInstanceId = @WorkflowId ", new { WorkflowId = workflow.Id}, await DbProvider.GetTransactionAsync());
        }
    }
}
