using Framework.AspNetCore.ExceptionHandling;
using Framework.Core.Dependency;
using Framework.Core.Exceptions;
using Framework.ExceptionHandling;
using Framework.ExceptionHandling.Http;
using Framework.Http;
using Framework.Json;
using Framework.Web.AspNetCore.ExceptionHandling;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Mvc.ExceptionHandling
{
    public class ExceptionFilter : IAsyncExceptionFilter, ITransient
    {
        public async Task OnExceptionAsync(ExceptionContext context)
        {
            if (!ShouldHandleException(context))
            {
                return;
            }
            await HandleAndWrapException(context);
        }

        protected virtual bool ShouldHandleException(ExceptionContext context)
        {
            if (context.ActionDescriptor.IsControllerAction() &&
                context.ActionDescriptor.HasObjectResult())
            {
                return true;
            }

            if (context.HttpContext.Request.CanAccept(MimeTypes.Application.Json))
            {
                return true;
            }

            if (context.HttpContext.Request.IsAjax())
            {
                return true;
            }

            return false;
        }

        protected virtual async Task HandleAndWrapException(ExceptionContext context)
        {
            context.HttpContext.Response.Headers.Add(FrameworkHttpConsts.ErrorFormat, "true");
            context.HttpContext.Response.StatusCode = (int)context.HttpContext.RequestServices
                .GetRequiredService<IHttpExceptionStatusCodeFinder>()
                .GetStatusCode(context.HttpContext, context.Exception);

            var exceptionHandlingOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<ExceptionHandlingOptions>>().Value;
            var exceptionToErrorInfoConverter = context.HttpContext.RequestServices.GetRequiredService<IExceptionToErrorInfoConverter>();
            var remoteServiceErrorInfo = exceptionToErrorInfoConverter.Convert(context.Exception, exceptionHandlingOptions.SendExceptionsDetailsToClients);

            context.Result = new ObjectResult(new RemoteServiceErrorResponse(remoteServiceErrorInfo));

            var logLevel = context.Exception.GetLogLevel();

            var remoteServiceErrorInfoBuilder = new StringBuilder();
            remoteServiceErrorInfoBuilder.AppendLine($"---------- {nameof(RemoteServiceErrorInfo)} ----------");
            remoteServiceErrorInfoBuilder.AppendLine(context.HttpContext.RequestServices.GetRequiredService<IJsonSerializer>().Serialize(remoteServiceErrorInfo, indented: true));

            var logger = context.HttpContext.RequestServices.GetService<ILogger<ExceptionFilter>>()?? NullLogger<ExceptionFilter>.Instance;

            logger.LogWithLevel(logLevel, remoteServiceErrorInfoBuilder.ToString());

            logger.LogException(context.Exception, logLevel);

            await context.HttpContext.RequestServices.GetRequiredService<IExceptionNotifier>().NotifyAsync(new ExceptionNotificationContext(context.Exception));

            context.Exception = null; 
        }
    }
}
