using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Hosting
{
    public class AspNetCoreWebSocketMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public AspNetCoreWebSocketMiddleware(RequestDelegate next, ILogger<AspNetCoreWebSocketMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IEndpointRouter router)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }

            var endpoint = router.Find(context);
            try
            {
                if (endpoint != null)
                {
                    _logger.LogInformation("Invoking websocket endpoint: {endpointType} for {url}", endpoint.GetType().FullName, context.Request.Path.ToString());
                    var result = await endpoint.ProcessAsync(context);
                    if (result != null)
                    {
                        _logger.LogTrace("Invoking result: {type}", result.GetType().FullName);
                        await result.ExecuteAsync(context);
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                if (ex is OperationCanceledException)
                {
                    //can logger?
                }
                else {
                    _logger.LogCritical(ex, "Unhandled exception: {exception}", ex.Message);
                }
                
            }
        }
    }
}
