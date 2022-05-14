namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流节点信息 最小要求
    /// </summary>
    public class WorkflowNodeInfo
    {
        public string Id { get; set; }

        /// <summary>
        /// 节点名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
