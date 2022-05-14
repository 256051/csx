using Framework.Core;
using Framework.Core.Dependency;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Clients
{
    /// <summary>
    /// 本质上Hub类似于控制器,所以当客户端创建Hub时应当是创建一个新的连接
    /// todo  没有集成认证组件,导致需要传用户id,因为当前项目需要
    /// </summary>
    public class SignalRHubsManager: ISignalRHubsManager,ISingleton
    {
        private SingnalRClientOptionsProvider _singnalRClientOptionsProvider;
        public SignalRHubsManager(SingnalRClientOptionsProvider singnalRClientOptionsProvider)
        {
            _singnalRClientOptionsProvider = singnalRClientOptionsProvider;
        }

        /// <summary>
        /// 创建Hub连接
        /// </summary>
        /// <param name="userId">连接的用户id</param>
        /// <param name="hubName">希望连接的hub名称</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<HubConnection> CreateAsync(string userId,string hubName,CancellationToken cancellationToken=default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            if (string.IsNullOrEmpty(hubName))
                throw new ArgumentNullException(nameof(hubName));
            HubConnection hubConnection;
            try
            {
                var url =$"{ _singnalRClientOptionsProvider.Value.Server.EnsureEndsWith('/')}{hubName}?userid={userId}";
                hubConnection = new HubConnectionBuilder()
                       .WithUrl(url)
                       .AddMessagePackProtocol()
                       .Build();

                await StartAsync(hubConnection);

                //检测到断开连接时(大概率网络不稳定),等待2秒自动重连
                hubConnection.Closed += async (error) => {
                    await StartAsync(hubConnection);
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"创建hub connection异常,信息:{ex.Message},堆栈:{ex.StackTrace}");
            }
            return hubConnection;
        }

        /// <summary>
        /// 开启连接
        /// </summary>
        /// <param name="hubConnection"></param>
        /// <returns></returns>
        private async Task StartAsync(HubConnection hubConnection)
        {
            try
            {
                await hubConnection.StartAsync();
            }
            catch
            {
                await Task.Delay(_singnalRClientOptionsProvider.Value.ReConnectTimeSpan ?? 2000);
                await StartAsync(hubConnection);
            }
        }

        public async Task RemoveAsync(HubConnection hubConnection, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var retryCount = 0;
            //移除连接时可能网络不好,导致连接处于非建立连接状态,那么等待
            do
            {
                //等待配置的时间后,默认10秒,断开连接,并释放连接
                if (retryCount == 5)
                {
                    await StopAsync(hubConnection);
                    throw new Exception("客户端连接非正常断开");
                }
                await Task.Delay(_singnalRClientOptionsProvider.Value.ReConnectTimeSpan ?? 2000);
                retryCount++;
            }
            while (hubConnection.State != HubConnectionState.Connected);
            await StopAsync(hubConnection);
        }

        private async Task StopAsync(HubConnection hubConnection)
        {
            await hubConnection.StopAsync();
            await hubConnection.DisposeAsync();
        }
    }
}
