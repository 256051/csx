using Framework.DDD.Application.Contracts.Dtos;
using System;

namespace Framework.ExceptionHandling.Domain.Shared
{
    public class ExceptionPagedRequestDto : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 是否处理
        /// </summary>
        public bool? Handled { get; set; }

        /// <summary>
        /// 日志级别
        /// 0-追踪
        /// 1-调试
        /// 2-信息
        /// 3-警告
        /// 4-异常
        /// 5-不稳定
        /// </summary>
        public int? LogLevel { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
