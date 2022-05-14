using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Application.Hubs
{
    /// <summary>
    /// 进度Hub
    /// </summary>
    public class Progress : HubBase
    {
        private ILogger<Progress> _logger;

        public Progress(ILogger<Progress> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 推送进度
        /// 注:网页端的进度订阅,登录是以任务id登陆的所以通过Clients.User调用
        /// </summary>
        /// <param name="taskId">任务id</param>
        /// <param name="progress">进度</param>
        /// <returns></returns>
        public async Task PushProgressAsync(string taskId,int progress)
        {
            try
            {
                await Clients.User(taskId).SendAsync("PullProgress", progress);
            }
            catch (Exception ex)
            {
                _logger.LogError($"task id:{taskId} 推送进度值:{progress}失败,信息:{ex.Message},堆栈:{ex.StackTrace}");
            }
        }
    }
}
