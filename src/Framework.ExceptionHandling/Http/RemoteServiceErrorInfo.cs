using System;
using System.Collections;

namespace Framework.ExceptionHandling.Http
{
    /// <summary>
    /// 接口调用异常
    /// </summary>
    [Serializable]
    public class RemoteServiceErrorInfo
    {
        public string Code { get; set; }

        public string Message { get; set; }

        public string Details { get; set; }

        public IDictionary Data { get; set; }

        /// <summary>
        /// 实体验证异常
        /// </summary>
        public RemoteServiceValidationErrorInfo[] ValidationErrors { get; set; }

        public RemoteServiceErrorInfo()
        {

        }

        public RemoteServiceErrorInfo(string message, string details = null, string code = null)
        {
            Message = message;
            Details = details;
            Code = code;
        }
    }
}
