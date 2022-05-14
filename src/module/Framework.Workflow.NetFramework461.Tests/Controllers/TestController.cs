using EpsonBurn.Equipment.Domain;
using EpsonBurn.Equipment.Domain.Shared;
using Framework.Core.Dependency;
using Framework.Timing;
using Framework.Workflow.AspNetWebApi;
using Framework.Workflow.Domain.Shared;
using System.Threading.Tasks;
using System.Web.Http;

namespace Framework.Workflow.NetFramework461.Tests
{
    [RoutePrefix("test")]
    public class TestController : ApiController,IScoped
    {
        private ApplicationWorkflowManager<ApplicationWorkflow> _workflowManager;
        private ApplicationWorkflowInstanceManager<ApplicationWorkflow> _instanceWorkflowManager;
        private EquipmentManagementManager<ApplicationEpsonBurnEquipment> _equipmentManagementManager;
        private ApplicationWorkflowManager<ApplicationWorkflow> _applicationWorkflowManager;
        private IClock _clock;

        public TestController(ApplicationWorkflowManager<ApplicationWorkflow> workflowManager, EquipmentManagementManager<ApplicationEpsonBurnEquipment> equipmentManagementManager, IClock clock, ApplicationWorkflowManager<ApplicationWorkflow> applicationWorkflowManager)
        {
            _workflowManager = workflowManager;
            _equipmentManagementManager = equipmentManagementManager;
            _clock = clock;
            _applicationWorkflowManager = applicationWorkflowManager;
        }

        [HttpGet]
        [Route("publishworkflow")]
        public async Task<IHttpActionResult> PublishAsync(string workflowId)
        {
            return Json(await _workflowManager.FindByIdAsync(workflowId));
        }

        [HttpGet]
        [Route("instancesbyroutekey")]
        public async Task<IHttpActionResult> FindInstancesByRouteKey(string routeKey)
        {
            return Json(await _instanceWorkflowManager.FindInstancesByRouteKeyAsync(routeKey,default));
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
