using Microsoft.Extensions.Logging;

namespace Framework.Core.Logging
{
    public interface IHasLogLevel
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        LogLevel LogLevel { get; set; }
    }
}
