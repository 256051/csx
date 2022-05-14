using System;

namespace Framework.Timing
{
    /// <summary>
    /// 时间配置
    /// </summary>
    public class ClockOptions
    {
        public DateTimeKind Kind { get; set; }

        public ClockOptions()
        {
            Kind = DateTimeKind.Unspecified;
        }
    }
}
