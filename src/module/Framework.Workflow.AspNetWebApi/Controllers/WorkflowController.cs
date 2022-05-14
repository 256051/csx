using Framework.Core.Dependency;
using Framework.Timing;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Shared;
using Framework.Workflow.Domain.Shared.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Framework.Workflow.AspNetWebApi
{
    [RoutePrefix("workflow")]
    public class WorkflowController : ApiController,IScoped
    {
        private ApplicationWorkflowManager<ApplicationWorkflow> _applicationWorkflowManager;
        private ApplicationWorkflowInstanceManager<ApplicationWorkflowInstance> _applicationWorkflowInstanceManager;
        private IClock _clock;

        public WorkflowController(ApplicationWorkflowManager<ApplicationWorkflow> applicationWorkflowManager, ApplicationWorkflowInstanceManager<ApplicationWorkflowInstance> applicationWorkflowInstanceManager, IClock clock)
        {
            _applicationWorkflowManager = applicationWorkflowManager;
            _applicationWorkflowInstanceManager = applicationWorkflowInstanceManager;
            _clock = clock;
        }

        /// <summary>
        /// 发布工作流
        /// </summary>
        /// <param name="workflowId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("publish")]
        public async Task<IHttpActionResult> PublishAsync(PublishDto dto)
        {
            if (string.IsNullOrEmpty(dto.WorkflowId))
            {
                throw new ArgumentNullException(nameof(dto.WorkflowId));
            }
            if (string.IsNullOrEmpty(dto.RouteData))
            {
                throw new ArgumentNullException(nameof(dto.RouteData));
            }
            var workflow = await _applicationWorkflowManager.FindByIdAsync(dto.WorkflowId);
            if (workflow == null)
            {
                throw new InvalidOperationException("workflow does't existed");
            }

            var instanceName = $"{workflow?.Name}:实例{Guid.NewGuid()}";
            var workflowInstance = new ApplicationWorkflowInstance()
            {
                Id = Guid.NewGuid().ToString(),
                Name = instanceName,
                State = (int)WorkflowInstanceState.Pre,
                RouteKey= dto.RouteKey,
                CreateTime = _clock.Now
            };
            await _applicationWorkflowInstanceManager.CreateAsync(workflowInstance);

            await _applicationWorkflowManager.AddToWorkflowInstanceAsync(workflow, instanceName);

            var nodes = (await _applicationWorkflowManager.GetNodesAsync(workflow)).ToList();
            if (nodes.Count <= 0)
            {
                throw new InvalidOperationException($"the number of nodes less or equal zero,workflow :{workflow}");
            }

            var nodeInstanceInfos = new List<WorkflowNodeInstanceInfo>();
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var nodeInfo = new WorkflowNodeInstanceInfo()
                {
                    Id = Guid.NewGuid().ToString(),
                    WorkflowNode = node,
                    CreateTime = _clock.Now,
                    State = WorkflowNodeInstanceState.Pre,
                    RouteKey = $"{dto.RouteKey}_{node.Name}",
                    RouteData = i==0?dto.RouteData:null
                };
                nodeInstanceInfos.Add(nodeInfo);
            }

            for (var i = 0; i < nodeInstanceInfos.Count; i++)
            {
                if (i < nodeInstanceInfos.Count - 1)
                    nodeInstanceInfos[i].Next = nodeInstanceInfos[i + 1];
                if (i != 0)
                    nodeInstanceInfos[i].Prev = nodeInstanceInfos[i - 1];
            }
            await _applicationWorkflowInstanceManager.CreateNodeInstancesAsync(workflowInstance, nodeInstanceInfos);
            return Json(workflowInstance.Id);
        }

        [HttpGet]
        [Route("findbyroutekey")]
        public async Task<IHttpActionResult> FindInstancesByRouteKeyAsync(string routeKey)
        {
            return Json(await _applicationWorkflowInstanceManager.FindInstancesByRouteKeyAsync(routeKey, default));
        }

        [HttpGet]
        [Route("updatenodestate")]
        public async Task<IHttpActionResult> UpdateNodeStateAsync(string nodeInstanceId)
        {
            await _applicationWorkflowInstanceManager.UpdateNodeStateAsync(nodeInstanceId, WorkflowNodeInstanceState.Succeed,"描述");
            return Json("");
        }

        [HttpGet]
        [Route("createinstancelog")]
        public async Task<IHttpActionResult> CreateInstanceLogAsync(string nodeInstanceId, string content)
        {
            var instance = await _applicationWorkflowInstanceManager.FindWorkflowInstanceByIdAsync(nodeInstanceId);
            await _applicationWorkflowInstanceManager.CreateInstanceLogAsync(instance, new Domain.WorkflowInstanceLogInfo()
            {
                Content = content
            });
            return Json("");
        }
        [Route("FindLogsByWorkflowInstanceId"), HttpGet]
        public async Task<IHttpActionResult> FindLogsByWorkflowInstanceId(string nodeInstanceId)
        {
            var instance = await _applicationWorkflowInstanceManager.FindLogsByWorkflowInstanceIdAsync(nodeInstanceId);

            return Json(new { success = true, data = instance });
        }
        [Route("FindByWorkflowInstanceId"), HttpGet]
        public async Task<IHttpActionResult> FindByWorkflowInstanceId(string nodeInstanceId) 
        {
            var instance = await _applicationWorkflowInstanceManager.FindByWorkflowInstanceIdAsync(nodeInstanceId);
            return Json(new { success = true, data = instance });
        }

        [Route("updatenode"), HttpGet]
        public async Task<IHttpActionResult> UpdateNodeAsync(string nodeInstanceId,string routeData)
        {
            var instance = await _applicationWorkflowInstanceManager.FindNodeByIdAsync(nodeInstanceId);
            instance.RouteData = routeData;
            await _applicationWorkflowInstanceManager.UpdateNodeAsync(instance);
            return Json(new { success = true, data = instance });
        }

        /// <summary>
        /// 更新节点状态
        /// </summary>
        /// <param name="nodeInstanceId"></param>
        /// <returns></returns>
        [Route("updateWorkflowNodeStatus"), HttpGet]
        public async Task<IHttpActionResult> UpdateWorkflowNodeStatus(string nodeInstanceId) 
        {
            await _applicationWorkflowManager.UpdateWorkflowNodeStatus(new ApplicationWorkflow() { Id = nodeInstanceId });
            return Json(new { success = true });
        } 
    }
}
