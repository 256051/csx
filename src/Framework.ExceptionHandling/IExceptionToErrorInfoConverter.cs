using Framework.ExceptionHandling.Http;
using System;

namespace Framework.ExceptionHandling
{
    public interface IExceptionToErrorInfoConverter
    {
        /// <summary>
        /// 异常包装
        /// </summary>
        /// <param name="exception">原生异常</param>
        /// <param name="includeSensitiveDetails">输出是否包含异常详情</param>
        /// <returns></returns>
        RemoteServiceErrorInfo Convert(Exception exception, bool includeSensitiveDetails=true);
    }
}
