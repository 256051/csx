namespace Framework.Uow
{
    /// <summary>
    /// 工作单元是否开启
    /// </summary>
    public enum UnitOfWorkTransactionBehavior
    {
        /// <summary>
        /// 计算得出
        /// </summary>
        Calculate,

        /// <summary>
        /// 开启
        /// </summary>
        Enabled,

        /// <summary>
        /// 关闭
        /// </summary>
        Disabled
    }
}
