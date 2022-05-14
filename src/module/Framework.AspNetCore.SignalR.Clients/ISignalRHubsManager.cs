using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Clients
{
    public interface ISignalRHubsManager
    {
        /// <summary>
        ///创建并初始化开启一个连接
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="hubName">hub名称</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HubConnection> CreateAsync(string userId, string hubName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除连接
        /// </summary>
        /// <param name="hubConnection"></param>
        /// <returns></returns>
        Task RemoveAsync(HubConnection hubConnection, CancellationToken cancellationToken = default);
    }
}
