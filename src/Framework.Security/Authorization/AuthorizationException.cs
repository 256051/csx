using Framework.Core;
using Framework.Core.Logging;
using Microsoft.Extensions.Logging;
using System;

namespace Framework.Security.Authorization
{
    /// <summary>
    /// 认证异常
    /// </summary>
    public class AuthorizationException : FrameworkException, IHasLogLevel, IHasErrorCode
    {
        /// <summary>
        /// 日志级别
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 异常编码
        /// </summary>
        public string Code { get; }
    }
}
