namespace Framework.AspNetCore.SignalR.Application.Models
{
    /// <summary>
    /// 多媒体消息返回
    /// </summary>
    public class MediaMessageResponse: MessageResponse
    {
        /// <summary>
        /// 媒体文件路径
        /// </summary>
        public string MediaFilePath { get; set; }
    }
}
