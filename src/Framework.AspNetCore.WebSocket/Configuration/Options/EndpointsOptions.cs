namespace Framework.AspNetCore.WebSocket.Configuration
{
    public class EndpointsOptions
    {
        /// <summary>
        /// 是否启用WebScoket数据拉取功能
        /// </summary>
        public bool EnableWebSocketPullEndpoint { get; set; } = true;

        /// <summary>
        /// 是否启用聊天功能
        /// </summary>
        public bool EnableTalkEndpoint { get; set; } = true;
    }
}
