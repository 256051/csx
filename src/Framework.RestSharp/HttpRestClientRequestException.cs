using Microsoft.Extensions.Logging;
using System;
using System.Runtime.Serialization;

namespace Framework.RestSharp
{
    /// <summary>
    /// rest请求异常
    /// </summary>
    public class HttpRestClientRequestException:Exception
    {
        public string Code
        {
            get;
            set;
        }

        public string Details
        {
            get;
            set;
        }

        public LogLevel LogLevel
        {
            get;
            set;
        }

        public HttpRestClientRequestException(string code = null, string message = null, string details = null, Exception innerException = null, LogLevel logLevel = LogLevel.Error)
            : base(message, innerException)
        {
            Code = code;
            Details = details;
            LogLevel = logLevel;
        }

        public HttpRestClientRequestException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        public HttpRestClientRequestException WithData(string name, object value)
        {
            Data[name] = value;
            return this;
        }
    }
}
