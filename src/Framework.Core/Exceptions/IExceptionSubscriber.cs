using System.Threading.Tasks;

namespace Framework.Core.Exceptions
{
    public interface IExceptionSubscriber
    {
        Task HandleAsync(ExceptionNotificationContext context);
    }
}
