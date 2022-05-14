using System;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流实例信息 最小要求
    /// </summary>
    public class WorkflowNodeInstanceInfo
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 当前节点实例所属工作流实例
        /// </summary>
        public WorkflowInstanceInfo WorkflowInstance { get; set; }

        /// <summary>
        /// 下一节点实例
        /// </summary>
        public WorkflowNodeInstanceInfo Next { get; set; }

        /// <summary>
        /// 上一节点实例
        /// </summary>
        public WorkflowNodeInstanceInfo Prev { get; set; }

        /// <summary>
        /// 当前节点
        /// </summary>
        public WorkflowNodeInfo WorkflowNode { get; set; }

        /// <summary>
        /// 工作流节点实例状态
        /// </summary>
        public WorkflowNodeInstanceState State { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 路由键
        /// </summary>
        public string RouteKey { get; set; }

        /// <summary>
        /// 路由数据
        /// </summary>
        public string RouteData { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
