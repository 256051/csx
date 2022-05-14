using System.Threading.Tasks;

namespace Framework.Core.Exceptions
{
    public interface IExceptionNotifier
    {
        Task NotifyAsync(ExceptionNotificationContext context);
    }
}
