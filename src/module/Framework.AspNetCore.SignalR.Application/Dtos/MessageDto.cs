using System;
using System.ComponentModel.DataAnnotations;

namespace Framework.AspNetCore.SignalR.Application.Models
{
    /// <summary>
    /// 消息查询
    /// </summary>
    public class MessageQueryDto
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Required(ErrorMessage ="用户Id不能为空")]
        public string UserId { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; } = DateTime.Now.AddDays(-3);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; } = DateTime.Now;
    }

    public class MessageQueryResultDto
    { 
        /// <summary>
        /// 消息Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        public DateTime SendTime { get; set; }

        /// <summary>
        /// 是否确认
        /// </summary>
        public bool? Confirmed { get; set; }
    }
}
