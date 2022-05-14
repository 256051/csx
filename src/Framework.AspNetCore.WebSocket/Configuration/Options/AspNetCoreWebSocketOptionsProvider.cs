using Framework.Core.Dependency;
using Microsoft.Extensions.Options;

namespace Framework.AspNetCore.WebSocket.Configuration
{
    public class AspNetCoreWebSocketOptionsProvider
    {
        public const string ConfigurationKey = "WebSocketOptions";

        private AspNetCoreWebSocketOptions WebSocketOptions { get; set; }

        private AspNetCoreWebSocketOptions _aspNetCoreWebSocketOptions;
        public AspNetCoreWebSocketOptions Value
        {
            get
            {
                if (_aspNetCoreWebSocketOptions == null)
                {
                    ValidWebSocketOptions(WebSocketOptions);
                    _aspNetCoreWebSocketOptions = WebSocketOptions;
                }
                return _aspNetCoreWebSocketOptions;
            }
        }

        public AspNetCoreWebSocketOptionsProvider(IOptions<AspNetCoreWebSocketOptions> apNetCoreWebSocketOptions)
        {
            WebSocketOptions = apNetCoreWebSocketOptions.Value;
        }

        /// <summary>
        /// 校验基本阐述的准确性
        /// </summary>
        private void ValidWebSocketOptions(AspNetCoreWebSocketOptions options)
        { 
            
        }
    }
}
