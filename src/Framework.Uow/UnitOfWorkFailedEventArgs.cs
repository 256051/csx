using System;

namespace Framework.Uow
{
    /// <summary>
    /// 工作单元执行失败事件
    /// </summary>
    public class UnitOfWorkFailedEventArgs : UnitOfWorkEventArgs
    {
        /// <summary>
        /// 捕获的异常
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// 是否需要rolledback
        /// </summary>
        public bool IsRolledback { get; }

        public UnitOfWorkFailedEventArgs(IUnitOfWork unitOfWork,Exception exception, bool isRolledback) : base(unitOfWork)
        {
            Exception = exception;
            IsRolledback = isRolledback;
        }
    }
}
