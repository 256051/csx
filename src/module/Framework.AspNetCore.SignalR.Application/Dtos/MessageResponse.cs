using System;

namespace Framework.AspNetCore.SignalR.Application.Models
{
    /// <summary>
    /// 消息返回
    /// </summary>
    public class MessageResponse
    {
        /// <summary>
        /// 消息Id
        /// </summary>
        public string MessageId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 发送者Id
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// 发送者名称
        /// </summary>
        public string SenderName { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// 确认时间
        /// </summary>
        public DateTime? ConfirmedTime { get; set; }
    }
}
