using System.Threading;
using System.Threading.Tasks;

namespace Framework.Uow
{
    /// <summary>
    /// 支持事务回滚
    /// </summary>
    public interface ISupportsRollback
    {
        Task RollbackAsync(CancellationToken cancellationToken);
    }
}
