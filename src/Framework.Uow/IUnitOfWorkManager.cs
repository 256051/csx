namespace Framework.Uow
{
    /// <summary>
    /// 工作单元管理类
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// 当前工作单元
        /// </summary>
        IUnitOfWork Current { get;  }

        //todo 嵌套
        /// <summary>
        /// 开启工作单元
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        IUnitOfWork Begin(UnitOfWorkOptions options);

        /// <summary>
        /// 最外部工作单元创建
        /// </summary>
        /// <param name="reservationName"></param>
        /// <param name="requiresNew"></param>
        /// <returns></returns>
        IUnitOfWork Reserve(string reservationName, bool requiresNew = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reservationName"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        bool TryBeginReserved(string reservationName, UnitOfWorkOptions options);
    }
}
