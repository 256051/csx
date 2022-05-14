using Framework.Core.Dependency;
using Framework.Core.Exceptions;
using Framework.Domain.Entities;
using Framework.Security.Authorization;
using Framework.Validation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Net;

namespace Framework.Web.AspNetCore.ExceptionHandling
{
    public class HttpExceptionStatusCodeFinder:IHttpExceptionStatusCodeFinder,ITransient
    {
        protected ExceptionHttpStatusCodeOptions Options { get; }

        public HttpExceptionStatusCodeFinder(
            IOptions<ExceptionHttpStatusCodeOptions> options)
        {
            Options = options.Value;
        }

        public virtual HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception)
        {
            if (exception is IHasHttpStatusCode exceptionWithHttpStatusCode &&
                exceptionWithHttpStatusCode.HttpStatusCode > 0)
            {
                return (HttpStatusCode)exceptionWithHttpStatusCode.HttpStatusCode;
            }

            if (exception is IHasErrorCode exceptionWithErrorCode &&
                !string.IsNullOrWhiteSpace(exceptionWithErrorCode.Code))
            {
                if (Options.ErrorCodeToHttpStatusCodeMappings.TryGetValue(exceptionWithErrorCode.Code, out var status))
                {
                    return status;
                }
            }

            if (exception is AuthorizationException)
            {
                return httpContext.User.Identity.IsAuthenticated
                    ? HttpStatusCode.Forbidden
                    : HttpStatusCode.Unauthorized;
            }

            //实体验证异常直接终端请求,所以返回400
            if (exception is FrameworkValidationException)
            {
                return HttpStatusCode.BadRequest;
            }

            if (exception is EntityNotFoundException)
            {
                return HttpStatusCode.NotFound; 
            }

            if (exception is NotImplementedException)
            {
                return HttpStatusCode.NotImplemented;
            }

            if (exception is IBusinessException)
            {
                return HttpStatusCode.Forbidden;
            }

            return HttpStatusCode.InternalServerError;
        }
    }
}
