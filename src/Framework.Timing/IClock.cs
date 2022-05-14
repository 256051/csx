using System;

namespace Framework.Timing
{
    public interface IClock
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// 时间类型
        /// </summary>
        DateTimeKind Kind { get; }

        /// <summary>
        /// 是否支持多时区
        /// </summary>
        bool SupportsMultipleTimezone { get; }

        /// <summary>
        /// 标准化
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        DateTime Normalize(DateTime dateTime);
    }
}
