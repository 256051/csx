using Framework.Core.Dependency;
using Microsoft.Extensions.Options;
using System;

namespace Framework.AspNetCore.SignalR.Clients
{
    public class SingnalRClientOptionsProvider:ISingleton
    {
        /// <summary>
        /// 安全的配置读取
        /// </summary>
        public SingnalRClientOptions Value
        {
            get
            {
                CheckValid();
                return _singnalRClientOptions;
            }
        }

        private SingnalRClientOptions _singnalRClientOptions;
        public SingnalRClientOptionsProvider(IOptions<SingnalRClientOptions> options)
        {
            _singnalRClientOptions = options.Value;
        }

        /// <summary>
        /// 检查配置有效性
        /// </summary>
        private void CheckValid()
        {
            if (_singnalRClientOptions == null)
                throw new Exception("未从配置文件中读取到配置");
            if (string.IsNullOrEmpty(_singnalRClientOptions.Server))
                throw new Exception("未从配置文件中读取到SignalR服务端地址");
        }
    }
}
