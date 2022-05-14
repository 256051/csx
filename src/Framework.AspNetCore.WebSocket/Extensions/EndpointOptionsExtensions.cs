using Framework.AspNetCore.WebSocket.Configuration;
using Framework.AspNetCore.WebSocket.Hosting;

namespace Framework.AspNetCore.WebSocket.Extensions
{
    public static class EndpointOptionsExtensions
    {
        /// <summary>
        /// 判断终结点的设置是否开启
        /// </summary>
        /// <param name="endpointsOptions"></param>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        public static bool IsEndpointEnabled(this EndpointsOptions endpointsOptions, Endpoint endpoint)
        {
            return endpoint?.Name switch
            {
                EndpointNames.WebSocketPull=> endpointsOptions.EnableWebSocketPullEndpoint,
                _ =>true
            };
        }
    }
}
