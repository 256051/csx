using Framework.Core.Dependency;
using Framework.Core.Exceptions;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Test
{
    public class LogtisticExceptionSubscriber : IExceptionSubscriber,ITransient
    {
        public Task HandleAsync(ExceptionNotificationContext context)
        {
            return Task.CompletedTask;
        }
    }
}
