using Dapper.Contrib.Extensions;
using Framework.DDD.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace Framework.ExceptionHandling.Dapper
{
    [Table("ExceptionInfo")]
    public class ExceptionInfo: Entity<Guid>
    {
        [ExplicitKey]
        public override Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 异常详情
        /// </summary>
        public string Message { get; set; }

        public ExceptionInfo()
        {

        }

        public ExceptionInfo(Exception ex) : this(Microsoft.Extensions.Logging.LogLevel.Error,ex)
        {
            
        }

        public ExceptionInfo(Exception ex, bool? handled = false) : this(Microsoft.Extensions.Logging.LogLevel.Error, ex, handled)
        {

        }

        public ExceptionInfo(LogLevel level,Exception ex):this(ex.Message, (int)level, false, ex.Source, ex.StackTrace)
        {

        }

        public ExceptionInfo(LogLevel level, Exception ex, bool? handled=false) : this(ex.Message, (int)level, handled, ex.Source, ex.StackTrace)
        {

        }

        public ExceptionInfo(string message, int logLevel, bool? handled, string source, string stackTrace)
        {
            Message = message;
            LogLevel = logLevel;
            Handled = handled;
            Source = source;
            StackTrace = stackTrace;
        }

        /// <summary>
        /// 日志级别
        /// </summary>
        public int LogLevel { get; set; }
        
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
    }
}
