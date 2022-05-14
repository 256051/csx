using Framework.Core;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpRequestExtensions
    {
        private const string RequestedWithHeader = "X-Requested-With";
        private const string XmlHttpRequest = "XMLHttpRequest";

        /// <summary>
        /// 判断当前请求是否是ajax请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsAjax(this HttpRequest request)
        {
            Check.NotNull(request, nameof(request));

            if (request.Headers == null)
            {
                return false;
            }

            return request.Headers[RequestedWithHeader] == XmlHttpRequest;
        }

        /// <summary>
        /// 判断当前请求是否含有对应的Accept
        /// </summary>
        /// <param name="request"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static bool CanAccept(this HttpRequest request,string contentType)
        {
            Check.NotNull(request, nameof(request));
            Check.NotNull(contentType, nameof(contentType));

            return request.Headers["Accept"].ToString().Contains(contentType);
        }
    }
}
