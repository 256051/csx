namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流节点实例状态
    /// </summary>
    public enum WorkflowNodeInstanceState
    {
        /// <summary>
        /// 待执行
        /// </summary>
        Pre=0,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing=1,
        /// <summary>
        /// 执行成功
        /// </summary>
        Succeed=2,
        /// <summary>
        /// 执行失败
        /// </summary>
        Failed=3
    }
}
