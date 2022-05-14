using Framework.Core.Dependency;
using Microsoft.AspNetCore.SignalR;
using System;

namespace Framework.AspNetCore.SignalR.Security
{
    /// <summary>
    /// //因为没有引入认证中间件,所以建立连接时上下文中的User为null,这里改成从连接字符串中获取,认证就走业务平台的体系
    /// </summary>
    public class UserIdProvider : IUserIdProvider,IReplace
    {
        public string GetUserId(HubConnectionContext connection)
        {
            var userId = connection?.GetHttpContext()?.Request?.Query["userid"];
            if (!userId.HasValue)
            {
                throw new Exception("客户端连接用户id不能为空,请在连接字符串中设置");
            }
            return userId.Value;
        }
    }
}
