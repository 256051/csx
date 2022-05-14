using Framework.Core;
using System;

namespace Framework.Uow
{
    /// <summary>
    /// 工作单元事件参数
    /// </summary>
    public class UnitOfWorkEventArgs : EventArgs
    {
        public IUnitOfWork UnitOfWork { get; }

        public UnitOfWorkEventArgs(IUnitOfWork unitOfWork)
        {
            Check.NotNull(unitOfWork, nameof(unitOfWork));

            UnitOfWork = unitOfWork;
        }
    }
}
