namespace Framework.Workflow.Domain.Shared.Dto
{
    /// <summary>
    /// 工作流发布
    /// </summary>
    public class PublishDto
    {
        /// <summary>
        /// 工作流Id
        /// </summary>
        public string WorkflowId { get; set; }

        /// <summary>
        /// 工作流实例节点的路由数据
        /// </summary>
        public string RouteData { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RouteKey { get; set; }
    }
}
