namespace Framework.Tasks.Domain
{
    /// <summary>
    /// 任务状态
    /// </summary>
    public enum TaskState
    {
        /// <summary>
        /// 预执行
        /// </summary>
        Pre=0,
        /// <summary>
        /// 执行中
        /// </summary>
        Executing=1,
        /// <summary>
        /// 执行成功
        /// </summary>
        Success=2,
        /// <summary>
        /// 执行失败
        /// </summary>
        Fail=3
    }
}
