using Framework.Core.Dependency;
using Framework.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.ExceptionHandling.Dapper
{
    public class DapperExceptionSubscriber : IExceptionSubscriber,ITransient
    {
        private IExceptionRepository _exceptionRepository;
        private List<Type> _ignoreTypes;
        public DapperExceptionSubscriber(IExceptionRepository exceptionRepository)
        {
            _exceptionRepository = exceptionRepository;
            _ignoreTypes = new List<Type>() { 
                typeof(UserFriendlyException)
            };
        }

        public Task HandleAsync(ExceptionNotificationContext context)
        {
            if (!_ignoreTypes.Contains(context.Exception.GetType()))
            {
                return _exceptionRepository.InsertAsync(new
               ExceptionInfo(context.Exception.Message,
               (int)context.LogLevel,
               false,
               context.Exception.Source,
               context.Exception.StackTrace));
            }
            return Task.CompletedTask;
        }
    }
}
