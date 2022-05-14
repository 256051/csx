using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Uow
{
    /// <summary>
    /// 事务Api抽象
    /// </summary>
    public interface ITransactionApi: IDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
    }
}
