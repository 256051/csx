namespace Framework.Workflow.Domain
{
    /// <summary>
    /// 工作流程实例执行状态
    /// </summary>
    public enum WorkflowInstanceState
    {
        /// <summary>
        /// 待执行
        /// </summary>
        Pre=0,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing = 1,
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
