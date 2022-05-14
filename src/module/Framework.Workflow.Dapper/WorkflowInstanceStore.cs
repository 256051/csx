using Dapper;
using Dapper.Contrib.Extensions;
using Framework.Core.Data;
using Framework.Timing;
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
    public class WorkflowInstanceStore<TWorkflowInstance, TKey> : WorkflowInstanceStore<TWorkflowInstance, TKey, WorkflowNodeInstance<TKey>, WorkflowInstanceLog<TKey>>
        where TWorkflowInstance : WorkflowInstance<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public WorkflowInstanceStore(IDbProvider dbProvider, IClock clock) :base(dbProvider, clock)
        { 
            
        }
    }

    public class WorkflowInstanceStore<TWorkflowInstance, TKey, TWorkflowNodeInstance, TWorkflowInstanceLog> : WorkflowInstanceStoreBase<TWorkflowInstance, TKey, TWorkflowNodeInstance, TWorkflowInstanceLog>
        where TWorkflowInstance : WorkflowInstance<TKey>, new()
        where TWorkflowNodeInstance : WorkflowNodeInstance<TKey>, new()
        where TWorkflowInstanceLog : WorkflowInstanceLog<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        protected IDbProvider DbProvider { get; set; }

        public WorkflowInstanceStore(IDbProvider dbProvider, IClock clock) : base(clock)
        {
            DbProvider = dbProvider;
        }

        public override async Task CreateAsync(TWorkflowInstance workflowInstance,CancellationToken cancellationToken=default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (workflowInstance is null)
                throw new ArgumentNullException(nameof(workflowInstance));
            var connection = await DbProvider.GetConnectionAsync();
            await connection.InsertAsync(workflowInstance, await DbProvider.GetTransactionAsync());
        }

        public override async Task CreateNodesAsync(TWorkflowInstance workflowInstance,IEnumerable<WorkflowNodeInstanceInfo> nodeInstanceInfos, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (workflowInstance is null)
                throw new ArgumentNullException(nameof(workflowInstance));
            if (nodeInstanceInfos.Count() <= 0)
                throw new InvalidOperationException("workflown node instances count less or equal 0");
            var connection = await DbProvider.GetConnectionAsync();
            var instances = nodeInstanceInfos.Select(s => CreateWorkflowNodeInstances(workflowInstance, s)).ToList();
            await connection.InsertAsync(instances, await DbProvider.GetTransactionAsync());
        }

        protected virtual ApplicationWorkflowNodeInstance CreateWorkflowNodeInstances(TWorkflowInstance workflowInstance, WorkflowNodeInstanceInfo workflowNodeInstanceInfo)
        {
            var info=CreateWorkflowNodeInstances(workflowNodeInstanceInfo);
            info.WorkflowInstanceId = workflowInstance.Id.ToString();
            return info;
        }

        protected virtual ApplicationWorkflowNodeInstance CreateWorkflowNodeInstances(WorkflowNodeInstanceInfo workflowNodeInstanceInfo)
        {
            return new ApplicationWorkflowNodeInstance()
            {
                WorkflowInstanceId = workflowNodeInstanceInfo.WorkflowInstance.Id,
                Id = workflowNodeInstanceInfo.Id,
                PrevId = workflowNodeInstanceInfo.Prev?.Id,
                NextId = workflowNodeInstanceInfo.Next?.Id,
                WorkflowNodeId = workflowNodeInstanceInfo.WorkflowNode.Id,
                CreateTime = workflowNodeInstanceInfo.CreateTime,
                State = (int)workflowNodeInstanceInfo.State,
                RouteKey = workflowNodeInstanceInfo.RouteKey,
                EndTime= workflowNodeInstanceInfo.EndTime,
                RouteData = workflowNodeInstanceInfo.RouteData
            };
        }

        public override async Task<IEnumerable<WorkflowNodeInstanceInfo>> FindByRouteKeyAndStateAsync(string routeKey, WorkflowNodeInstanceState state, CancellationToken cancellationToken=default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(routeKey))
                throw new ArgumentNullException(nameof(routeKey));
            var connection = await DbProvider.GetConnectionAsync();
            var nodeInstances= await connection.QueryAsync<ApplicationWorkflowNodeInstance>("select * from ServiceWorkflowNodeInstances where RouteKey=@routeKey and State=@State", new { routeKey, State = (int)state }, await DbProvider.GetTransactionAsync());
         
            return await CreateWorkflowNodeInstanceInfos(nodeInstances.ToList());
        }


        public override async Task UpdateStateAsync(string id, WorkflowInstanceState state,  CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var flag = true;
            var connection = await DbProvider.GetConnectionAsync();
            if (state == WorkflowInstanceState.Succeed)
            {
                if (!await NodeAllSucceed(id, cancellationToken))
                    flag = false;
            }
            if (flag)
            {
                var sql = "update ServiceWorkflowInstances set State=@State,EndTime=@EndTime  where Id=@id";
                await connection.ExecuteAsync(sql, new { State = (int)state, id, EndTime= Clock.Now }, await DbProvider.GetTransactionAsync());
            }
        }

        /// <summary>
        /// 判断流程实例下面的节点实例有没有全部完成
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> NodeAllSucceed(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            var nodeInstances = await connection.QueryAsync<ApplicationWorkflowNodeInstance>("select * from ServiceWorkflowNodeInstances where WorkflowInstanceId=@id", new { id }, await DbProvider.GetTransactionAsync());
            var result = nodeInstances.Where(s => !(CreateWorkflowInstanceState(s.State).Equals(WorkflowInstanceState.Succeed)));
            return result.Count()==0;
        }

        /// <summary>
        /// 更新工作流实例状态通过工作流节点实例状态
        /// </summary>
        /// <returns></returns>
        private async Task UpdateWorkflowStateByNodeAsync(string nodeInstanceId, WorkflowNodeInstanceState state, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            var nodeInstance = await connection.QueryFirstOrDefaultAsync<ApplicationWorkflowNodeInstance>("select * from serviceworkflownodeinstances where Id=@nodeInstanceId",new { nodeInstanceId },await DbProvider.GetTransactionAsync());
            if (nodeInstance == null)
                throw new ArgumentNullException(nameof(nodeInstance));
            await UpdateStateAsync(nodeInstance.WorkflowInstanceId, ConvertFromNodeState(state), cancellationToken);
        }

        private WorkflowInstanceState ConvertFromNodeState(WorkflowNodeInstanceState workflowNodeInstanceState)
        {
            switch (workflowNodeInstanceState)
            {
                case WorkflowNodeInstanceState.Pre:return WorkflowInstanceState.Pre;
                case WorkflowNodeInstanceState.Executing: return WorkflowInstanceState.Executing;
                case WorkflowNodeInstanceState.Failed: return WorkflowInstanceState.Failed;
                case WorkflowNodeInstanceState.Succeed: return WorkflowInstanceState.Succeed;
                default:throw new InvalidOperationException("WorkflowInstanceState convert failed");
            }
        }

        /// <summary>
        /// 如果同一时间,其他用户修改了节点实例状态,当前用户已经进入事务,则会存在状态丢失问题,所以这里需要加锁
        /// </summary>
        private static SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        public override async Task UpdateNodeStateAsync(string nodeInstanceId, WorkflowNodeInstanceState state, string description, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            try
            {
                await _semaphoreSlim.WaitAsync();
                var sql = "update ServiceWorkflowNodeInstances set State=@State,Description=@description,EndTime=@EndTime where Id=@nodeInstanceId";
                await connection.ExecuteAsync(sql, new { State = ((int)state), description, nodeInstanceId, EndTime = Clock.Now }, await DbProvider.GetTransactionAsync());
                await UpdateWorkflowStateByNodeAsync(nodeInstanceId, state, cancellationToken);
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private async Task<List<WorkflowNodeInstanceInfo>> CreateWorkflowNodeInstanceInfos(List<ApplicationWorkflowNodeInstance> applicationWorkflowNodeInstances)
        {
            var result = new List<WorkflowNodeInstanceInfo>();
            var connection = await DbProvider.GetConnectionAsync();
            var workflowInstanceIds = applicationWorkflowNodeInstances.Select(s => s.WorkflowInstanceId);
            var applicationWorkflowInstances= await connection.QueryAsync<ApplicationWorkflowInstance>("select * from ServiceWorkflowInstances where Id in @Ids", new { Ids = workflowInstanceIds }, await DbProvider.GetTransactionAsync());
            var prevNextNodeInstanceIds = applicationWorkflowNodeInstances.Select(instance => instance.PrevId).Union(applicationWorkflowNodeInstances.Select(instance => instance.NextId)).Distinct().Where(w => w != null);
            var prevNextNodeInstances = await connection.QueryAsync<ApplicationWorkflowNodeInstance>("select * from ServiceWorkflowNodeInstances where Id in @Ids", new { Ids = prevNextNodeInstanceIds }, await DbProvider.GetTransactionAsync());
            var workflowNodeIds = applicationWorkflowNodeInstances.Select(s => s.WorkflowNodeId).Union(prevNextNodeInstances.Select(s => s.WorkflowNodeId)).Distinct().ToList();
            var workflowNodes = (await connection.QueryAsync<ApplicationWorkflowNode>("select * from ServiceWorkflowNodes where Id in @Ids", new { Ids = workflowNodeIds }, await DbProvider.GetTransactionAsync())).ToList();
            foreach (var current in applicationWorkflowNodeInstances)
            {
                var nexts = prevNextNodeInstances.Where(w => w.Id.Equals(current.NextId));
                if (nexts.Count() > 1)
                {
                    throw new Exception($"{current} sibling child nodes  more than one");
                }
                var next = nexts.FirstOrDefault();
                var prevs = prevNextNodeInstances.Where(w => w.Id.Equals(current.PrevId));
                if (prevs.Count() > 1)
                {
                    throw new Exception($"{current} sibling parent nodes  more than one");
                }
                var prev = prevs.FirstOrDefault();

                result.Add(new WorkflowNodeInstanceInfo()
                {
                    Id= current.Id,
                    WorkflowInstance= CreateWorkflowInstanceInfo(applicationWorkflowInstances.Where(w=>w.Id== current.WorkflowInstanceId).FirstOrDefault()),
                    Next = next!=null? CreateWorkflowNodeInstanceInfo(next, workflowNodes): null,
                    Prev = prev != null ? CreateWorkflowNodeInstanceInfo(prev, workflowNodes) : null,
                    WorkflowNode = CreateWorkflowNode(current, workflowNodes),
                    State=CreateWorkflowNodeInstanceState(current.State),
                    CreateTime= current.CreateTime,
                    RouteData= current.RouteData,
                    EndTime= current.EndTime,
                    RouteKey =current.RouteKey
                });
            }
            return result;
        }

        private WorkflowNodeInstanceInfo CreateWorkflowNodeInstanceInfo(ApplicationWorkflowNodeInstance applicationWorkflowNodeInstance, List<ApplicationWorkflowNode> applicationWorkflowNodes)
        {
            return new WorkflowNodeInstanceInfo()
            {
                Id = applicationWorkflowNodeInstance.Id,
                Next = null,
                Prev = null,
                EndTime = applicationWorkflowNodeInstance.EndTime,
                RouteData = applicationWorkflowNodeInstance.RouteData,
                RouteKey= applicationWorkflowNodeInstance.RouteKey,
                WorkflowNode = CreateWorkflowNode(applicationWorkflowNodeInstance,applicationWorkflowNodes),
                State= CreateWorkflowNodeInstanceState(applicationWorkflowNodeInstance.State),
                CreateTime= applicationWorkflowNodeInstance.CreateTime
            };
        }

        private WorkflowNodeInfo CreateWorkflowNode(ApplicationWorkflowNodeInstance applicationWorkflowNodeInstance,List<ApplicationWorkflowNode> applicationWorkflowNodes)
        {
            applicationWorkflowNodes = applicationWorkflowNodes.Where(w => w.Id == applicationWorkflowNodeInstance.WorkflowNodeId).ToList();
            if (applicationWorkflowNodes.Count != 1)
                throw new Exception($"{applicationWorkflowNodeInstance} node info illegal");
            var applicationWorkflowNode = applicationWorkflowNodes.FirstOrDefault();
            return new WorkflowNodeInfo()
            {
                Id= applicationWorkflowNode.Id,
                Name= applicationWorkflowNode.Name,
                Sort= applicationWorkflowNode.Sort
            };
        }

        private WorkflowInstanceInfo CreateWorkflowInstanceInfo(ApplicationWorkflowInstance applicationWorkflowInstance)
        {
            return new WorkflowInstanceInfo()
            {
                Id= applicationWorkflowInstance.Id,
                State= CreateWorkflowInstanceState(applicationWorkflowInstance.State),
                CreateTime= applicationWorkflowInstance.CreateTime
            };
        }

        private WorkflowNodeInstanceState CreateWorkflowNodeInstanceState(int state)
        {
            switch (state)
            {
                case 0:return WorkflowNodeInstanceState.Pre;
                case 1: return WorkflowNodeInstanceState.Executing;
                case 2: return WorkflowNodeInstanceState.Succeed;
                case 3: return WorkflowNodeInstanceState.Failed;
                default:throw new Exception("WorkflowNodeInstanceState dose not existed");

            }
        }

        private WorkflowInstanceState CreateWorkflowInstanceState(int state)
        {
            switch (state)
            {
                case 0: return WorkflowInstanceState.Pre;
                case 1: return WorkflowInstanceState.Executing;
                case 2: return WorkflowInstanceState.Succeed;
                case 3: return WorkflowInstanceState.Failed;
                default: throw new Exception("WorkflowNodeInstanceState dose not existed");

            }
        }

        public override async Task CreateLogAsync(TWorkflowInstance workflowInstance, WorkflowInstanceLogInfo logInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var logger= CreateApplicationWorkflowInstanceLog(CreateWorkflowInstanceLog(workflowInstance, logInfo));
            var connection = await DbProvider.GetConnectionAsync();
            await connection.InsertAsync(logger, await DbProvider.GetTransactionAsync());
        }

        #region Dapper实体适配
        private ApplicationWorkflowInstanceLog CreateApplicationWorkflowInstanceLog(TWorkflowInstanceLog workflowInstanceLog)
        {
            return new ApplicationWorkflowInstanceLog()
            {
                Id = Guid.NewGuid().ToString(),
                WorkflowInstanceId = workflowInstanceLog.WorkflowInstanceId.ToString(),
                Content = workflowInstanceLog.Content,
                CreateTime = workflowInstanceLog.CreateTime
            };
        } 
        #endregion

        public override async Task<TWorkflowInstance> FindWorkflowInstanceByIdAsync(string nodeInstanceId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            var sql = @"SELECT
                        	instances.* 
                        FROM
                        	serviceworkflownodeinstances nodeInstances
                        	INNER JOIN serviceworkflowinstances instances ON nodeInstances.WorkflowInstanceId = instances.Id
                        	WHERE nodeInstances.Id=@Id";
            return await connection.QueryFirstOrDefaultAsync<TWorkflowInstance>(sql, new { Id = nodeInstanceId }, await DbProvider.GetTransactionAsync());
        }

        public override async Task<IEnumerable<WorkflowNodeInstanceInfo>> FindByWorkflowInstanceIdAsync(string workflowInstanceId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            var nodeInstances = await connection.QueryAsync<ApplicationWorkflowNodeInstance>("select * from ServiceWorkflowNodeInstances where WorkflowInstanceId=@workflowInstanceId", new { workflowInstanceId }, await DbProvider.GetTransactionAsync());
            return await CreateWorkflowNodeInstanceInfos(nodeInstances.ToList());
        }

        private WorkflowInstanceLogInfo CreateWorkflowInstanceLogInfo(ApplicationWorkflowInstanceLog applicationWorkflowInstanceLog)
        {
            return new WorkflowInstanceLogInfo()
            {
                Content= applicationWorkflowInstanceLog.Content,
                CreateTime= applicationWorkflowInstanceLog.CreateTime
            };
        }

        public override async Task<IEnumerable<WorkflowInstanceLogInfo>> FindLogsByWorkflowInstanceIdAsync(string workflowInstanceId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            var nodeInstances = await connection.QueryAsync<ApplicationWorkflowInstanceLog>("select * from ServiceWorkflowInstancesLogs where WorkflowInstanceId=@workflowInstanceId", new { workflowInstanceId }, await DbProvider.GetTransactionAsync());
            return nodeInstances.Select(s => CreateWorkflowInstanceLogInfo(s));
        }

        public override async Task<IEnumerable<TWorkflowInstance>> FindByIdListAsync(List<string> ids, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            var sql = "select * from ServiceWorkflowInstances where id in @ids";
            return await connection.QueryAsync<TWorkflowInstance>(sql, new {  ids }, await DbProvider.GetTransactionAsync());
            
        }

        public override async Task UpdateNodeAsync(WorkflowNodeInstanceInfo workflowNodeInstanceInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            ;
            var connection = await DbProvider.GetConnectionAsync();
            await connection.UpdateAsync(CreateWorkflowNodeInstances(workflowNodeInstanceInfo), transaction: await DbProvider.GetTransactionAsync());
        }

        public override async Task<WorkflowNodeInstanceInfo> FindNodeByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var connection = await DbProvider.GetConnectionAsync();
            var nodeInstances = await connection.QueryAsync<ApplicationWorkflowNodeInstance>("select * from ServiceWorkflowNodeInstances where Id=@id", new { id}, await DbProvider.GetTransactionAsync());
            return (await CreateWorkflowNodeInstanceInfos(nodeInstances.ToList())).FirstOrDefault();
        }
    }
}
 