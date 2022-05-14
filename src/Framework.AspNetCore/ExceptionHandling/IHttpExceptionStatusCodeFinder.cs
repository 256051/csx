using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace Framework.Web.AspNetCore.ExceptionHandling
{
    public interface IHttpExceptionStatusCodeFinder
    {
        HttpStatusCode GetStatusCode(HttpContext httpContext, Exception exception);
    }
}
