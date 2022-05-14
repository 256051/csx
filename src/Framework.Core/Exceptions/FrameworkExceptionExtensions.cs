using Framework.Core.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace System
{
    public static class FrameworkExceptionExtensions
    {
        /// <summary>
        /// 获取抛出异常中的日志级别
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="defaultLevel"></param>
        /// <returns></returns>
        public static LogLevel GetLogLevel(this Exception exception, LogLevel defaultLevel = LogLevel.Error)
        {
            return (exception as IHasLogLevel)?.LogLevel ?? defaultLevel;
        }
    }
}
