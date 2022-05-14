using System;

namespace Framework.ExceptionHandling.Http
{
    /// <summary>
    /// 实体验证异常
    /// </summary>
    [Serializable]
    public class RemoteServiceValidationErrorInfo
    {
        public string Message { get; set; }

        public string[] Members { get; set; }

        public RemoteServiceValidationErrorInfo()
        {

        }

        public RemoteServiceValidationErrorInfo(string message)
        {
            Message = message;
        }

        public RemoteServiceValidationErrorInfo(string message, string[] members)
            : this(message)
        {
            Members = members;
        }

        public RemoteServiceValidationErrorInfo(string message, string member) : this(message, new[] { member })
        {

        }
    }
}
