using System.Threading;
using System.Threading.Tasks;

namespace Framework.Uow
{
    /// <summary>
    /// 仓储是否支持SaveChange
    /// </summary>
    public interface ISupportsSavingChanges
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
