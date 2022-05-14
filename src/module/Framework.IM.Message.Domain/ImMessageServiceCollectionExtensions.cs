using Microsoft.Extensions.DependencyInjection;

namespace Framework.IM.Message.Domain
{
    public static class ImMessageServiceCollectionExtensions
    {
        /// <summary>
        /// 即时通信消息管理
        /// </summary>
        /// <typeparam name="TUserMessage"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static ImMessageBuilder AddImMessage<TUserMessage>(this IServiceCollection services) 
            where TUserMessage : class
        {
            services.AddScoped<UserMessageManager<TUserMessage>>();
            return new ImMessageBuilder(typeof(TUserMessage), services);
        }
    }
}
