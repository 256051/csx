namespace Framework.Uow
{
    /// <summary>
    /// 工作环境域内的工作单元处理者
    /// </summary>
    public interface IUnitOfWorkAccessor
    {
        /// <summary>
        /// 当前环境域内的工作单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }

        /// <summary>
        /// 设置当前环境域内的工作单元
        /// </summary>
        /// <param name="unitOfWork"></param>
        void SetUnitOfWork(IUnitOfWork unitOfWork);
    }
}
