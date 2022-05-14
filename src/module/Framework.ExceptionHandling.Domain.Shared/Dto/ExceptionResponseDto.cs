using Microsoft.Extensions.Logging;
using System;

namespace Framework.ExceptionHandling.Domain.Shared
{

    public class ExceptionResponseDto
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public string LogLevel { get; set; }

        /// <summary>
        /// 是否处理
        /// </summary>
        public bool? Handled { get; set; }

        /// <summary>
        /// 异常来源模块  应用级别
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 异常详情
        /// </summary>
        public string Message { get; set; }
    }
}
