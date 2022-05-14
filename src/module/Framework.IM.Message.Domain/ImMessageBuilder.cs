using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.IM.Message.Domain
{
    /// <summary>
    /// 工作流Builder
    /// </summary>
    public class ImMessageBuilder
    {
        public ImMessageBuilder(Type userMessageType,IServiceCollection services)
        {
            UserMessageType = userMessageType;
            Services = services;
        }

        public IServiceCollection Services { get; private set; }

        /// <summary>
        /// 用户消息类型
        /// </summary>
        public Type UserMessageType { get;private set; }
    }
}
