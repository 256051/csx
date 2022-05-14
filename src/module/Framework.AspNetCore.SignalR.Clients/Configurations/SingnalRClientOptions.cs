namespace Framework.AspNetCore.SignalR.Clients
{
    /// <summary>
    /// Singnalr客户端配置
    /// </summary>
    public class SingnalRClientOptions
    {
        /// <summary>
        /// 服务地址
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 重连时间间隔
        /// </summary>
        public int? ReConnectTimeSpan { get; set; }
    }
}
