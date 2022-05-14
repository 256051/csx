using Framework.AspNetCore.SignalR.Application.Services;
using Framework.Core.Configurations;
using Framework.Json;
using Framework.Security.Users;
using Framework.Uow;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Application.Hubs
{
    public abstract class HubBase:Hub
    {
        protected IServiceScopeFactory _serviceScopeFactory;
        private ILogger<HubBase> _logger;
        protected IUnitOfWorkManager _unitOfWorkManager;
        protected ICurrentUser _currentUser;
        protected IJsonSerializer _jsonSerializer;
        protected HubBase()
        {
            var provider = ApplicationConfiguration.Current.Provider;
            _serviceScopeFactory = provider.GetRequiredService<IServiceScopeFactory>();
            _logger = provider.GetRequiredService<ILogger<HubBase>>();
            _unitOfWorkManager = provider.GetRequiredService<IUnitOfWorkManager>();
            _currentUser = provider.GetRequiredService<ICurrentUser>();
            _jsonSerializer = provider.GetRequiredService<IJsonSerializer>();
        }

        /// <summary>
        /// singlr 连接到服务端发生异常,不会抛异常到客户端,这里catch异常写入日志
        /// </summary>
        /// <returns></returns>
        private async Task UserLoginAsync()
        {
            //因为没有引入认证中间件,所以hub连接成功,方便后续业务,即写入用户上下文,业务平台可能已经登录成功,所以可做可不做,后期需要优化
            var userId = Context?.GetHttpContext()?.Request?.Query["userid"].ToString();
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("客户端连接用户id不能为空,请在连接字符串中设置");
                throw new Exception("客户端连接用户id不能为空,请在连接字符串中设置");
            }
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions()))
                {
                    var userInfoProvider = scope.ServiceProvider.GetRequiredService<IUserInfoProvider>();
                    var userInfo = await userInfoProvider.FindByIdAsync(userId, Context.GetHttpContext().RequestAborted);
                    if (userInfo != null)
                    {
                        var identity = new ClaimsIdentity(new List<Claim>() {
                        new Claim(ClaimTypes.Sid,userInfo.UserId),
                        new Claim(ClaimTypes.Name,userInfo.UserName)
                     });
                        Context.GetHttpContext().User = new GenericPrincipal(identity, null);
                    }
                }
            }
        }

        /// <summary>
        /// 用户连接到服务器
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await UserLoginAsync();
            await Clients.All.SendAsync("OnConnected", $"{_currentUser.UserName}已上线");
        }

        /// <summary>
        /// 用户断开连接
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override Task OnDisconnectedAsync(Exception exception)
        {
            var name = Context.GetHttpContext().Request.Query["name"];
            return Clients.All.SendAsync("OnDisconnected", $"{name}已离线");
        }
    }
}
