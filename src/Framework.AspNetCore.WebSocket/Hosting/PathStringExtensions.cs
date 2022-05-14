using Framework.Core;
using Microsoft.AspNetCore.Http;

namespace Framework.AspNetCore.WebSocket.Hosting
{
    public static class PathStringExtensions
    {
        /// <summary>
        /// 获取WebSocket操作类型 跳转到对应的EndPoint pull-代表当前操作是拉取数据操作
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static string GetOperation(this PathString pathString)
        {
            if (!pathString.HasValue)
                return string.Empty;
            return pathString.Value.Split("/")[1].EnsureLeadingSlash();
        }

        /// <summary>
        /// 获取当前操作 对应的业务操作
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static string GetService(this PathString pathString)
        {
            if (!pathString.HasValue)
                return string.Empty;
            var paths=pathString.Value.Split("/");
            return paths[paths.Length-1].EnsureLeadingSlash();
        }

        /// <summary>
        /// 获取当前操作 对应的用户id
        /// </summary>
        /// <param name="pathString"></param>
        /// <returns></returns>
        public static string GetUserId(this PathString pathString)
        {
            if (!pathString.HasValue)
                return string.Empty;
            var paths = pathString.Value.Split("/");
            return paths[paths.Length - 1];
        }
    }
}
