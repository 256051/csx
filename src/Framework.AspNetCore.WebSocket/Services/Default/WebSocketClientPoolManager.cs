using Framework.AspNetCore.WebSocket.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.WebSocket.Services.Default
{
    /// <summary>
    /// WebSocket客户端连接管理
    /// </summary>
    /// <typeparam name="TWebSocketClient"></typeparam>
    public class WebSocketClientPoolManager<TWebSocketClient> where TWebSocketClient : WebSocketClient
    {
        protected List<TWebSocketClient> _clientPool;
        protected ILogger<WebSocketClientPoolManager<TWebSocketClient>> _logger;

        public WebSocketClientPoolManager(ILogger<WebSocketClientPoolManager<TWebSocketClient>> logger)
        {
            _clientPool = new List<TWebSocketClient>();
            _logger = logger;
        }

        private static object _addLock = new object();
        /// <summary>
        /// 新增客户端
        /// </summary>
        /// <param name="webSocketClient"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public  Task<bool> AddClientAsync(TWebSocketClient webSocketClient, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            lock (_addLock)
            {
                try
                {
                    _clientPool.Add(webSocketClient);
                    _logger.LogInformation($"新增客户端:{webSocketClient},当前客户端总数:{_clientPool.Count}");
                    return Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"{webSocketClient} Add Failed,异常信息:{ex.Message},堆栈信息:{ex.StackTrace}");
                    return Task.FromResult(false);
                }
            }
        }

        private static SemaphoreSlim _removeLock = new SemaphoreSlim(1,1);
        public async Task RemoveClientAsync(TWebSocketClient webSocketClient,string description=null)
        {
            if (IsValidClient(webSocketClient))
            {
                await _removeLock.WaitAsync();
                try
                {
                    _clientPool.Remove(webSocketClient);
                    await CloseClientAsync(webSocketClient, description);
                    _logger.LogInformation($"客户端:{webSocketClient}因为：{description}已移除,当前客户端总数:{_clientPool.Count}");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"client:{webSocketClient} remove failed 异常信息:{ex.Message},堆栈:{ex.StackTrace}");
                }
                _removeLock.Release();
            }
        }

        /// <summary>
        /// 检查客户端是否有效
        /// </summary>
        public virtual bool IsValidClient(TWebSocketClient webSocketClient)
        {
            var flag = false;
            if (webSocketClient.WebSocket == null)
            {
                _logger.LogError($"client:{webSocketClient} websocket is null");
            }
            else if (webSocketClient.ConnectionInfo == null)
            {
                _logger.LogError($"client:{webSocketClient} http connection is null");
            }
            else
            {
                flag = true;
            }
            return flag;
        }

        /// <summary>
        /// 关闭客户端并释放资源
        /// </summary>
        /// <param name="client"></param>
        private async Task CloseClientAsync(TWebSocketClient webSocketClient, string closeReason=null)
        {
            if (webSocketClient != null)
            {
                if (webSocketClient.WebSocket != null && webSocketClient.WebSocket.State != WebSocketState.Closed)
                {
                    try
                    {
                        await webSocketClient.WebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, closeReason ?? "", CancellationToken.None);
                        webSocketClient.WebSocket.Dispose();
                        webSocketClient.WebSocket = null;
                        webSocketClient = null;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"client:{webSocketClient} websocket close failed 异常信息:{ex.Message},堆栈:{ex.StackTrace}");
                    }
                }
            }
        }

    }
}
