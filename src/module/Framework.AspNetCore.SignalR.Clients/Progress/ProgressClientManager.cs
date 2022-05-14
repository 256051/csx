using Framework.Core.Dependency;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Clients
{
    public class ProgressClientManager: IProgressClientManager,ISingleton
    {
        private ISignalRHubsManager _signalRHubsManager;
        private ConcurrentDictionary<string, HubConnection> _progressClients = new ConcurrentDictionary<string, HubConnection>();
        private const string _progressHubName = "progress";
        private const string __progressMethod = "PushProgressAsync";
        private ILogger<ProgressClientManager> _logger;
        public ProgressClientManager(ISignalRHubsManager signalRHubsManager, ILogger<ProgressClientManager> logger)
        {
            _signalRHubsManager = signalRHubsManager;
            _logger = logger;
        }

        public async Task CreateAsync(string taskId,string userId,CancellationToken cancellactionToken=default)
        {
            if (!_progressClients.ContainsKey(taskId))
            {
                cancellactionToken.ThrowIfCancellationRequested();
                var hubConenction = await _signalRHubsManager.CreateAsync(userId, _progressHubName, cancellactionToken);
                if (!_progressClients.TryAdd(taskId, hubConenction))
                {
                    throw new Exception($"task id:{taskId} 持久化连接到进度任务hub连接池失败");
                }
            }
        }

        public async Task RemoveAsync(string taskId, CancellationToken cancellactionToken = default)
        {
            cancellactionToken.ThrowIfCancellationRequested();
            if (_progressClients.TryRemove(taskId, out var _hubConnection))
            {
                await _signalRHubsManager.RemoveAsync(_hubConnection, cancellactionToken);
            }
            else {
                throw new Exception($"task id:{taskId} 从到进度任务hub连接池移除连接失败");
            }
        }

        public async Task PushProgressAsync(string taskId, int progress, CancellationToken cancellactionToken = default)
        {
            cancellactionToken.ThrowIfCancellationRequested();
            if (_progressClients.TryGetValue(taskId, out var _hubConnection))
            {
                try
                {
                    await _hubConnection.InvokeAsync(__progressMethod, taskId, progress, cancellactionToken);
                }
                catch (Exception ex)
                {
                    //进度推送的频率很快,存在网络不稳定的情况,因为默认设置的重连间隔比较长(2秒),所以这里选择记录日志,而不是抛出异常
                    _logger.LogError($"task id:{taskId}进度推送异常,信息:{ex.Message},堆栈:{ex.StackTrace}");
                }
            }
        }

        public Task<bool> FindByTaskIdAsync(string taskId, CancellationToken cancellactionToken = default)
        {
            cancellactionToken.ThrowIfCancellationRequested();
            return Task.FromResult(_progressClients.ContainsKey(taskId));
        }
    }
}
