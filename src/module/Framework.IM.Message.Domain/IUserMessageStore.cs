using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.IM.Message.Domain
{
    /// <summary>
    /// 用户消息仓储
    /// </summary>
    /// <typeparam name="TUserMessage"></typeparam>
    public interface IUserMessageStore<TUserMessage> where TUserMessage : class
    {
        /// <summary>
        /// 获取消息接收者Id
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetReceiverIdAsync(TUserMessage userMessage,CancellationToken cancellationToken);

        /// <summary>
        /// 获取发送者Id
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetSenderIdAsync(TUserMessage userMessage, CancellationToken cancellationToken);

        /// <summary>
        /// 获取消息内容
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string> GetContentAsync(TUserMessage userMessage, CancellationToken cancellationToken);

        /// <summary>
        /// 持久化消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="receiverGroupId">消息接收组</param>
        /// <param name="senderId">消息发送者</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateMessageAsync(TUserMessage message,CancellationToken cancellationToken);

        /// <summary>
        /// 批量持久化消息
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task CreateMessagesAsync(IEnumerable<TUserMessage> messages, CancellationToken cancellationToken);

        /// <summary>
        /// 根据id查找消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<TUserMessage> FindByIdAsync(string messageId, CancellationToken cancellationToken);

        /// <summary>
        /// 更新消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task UpdateAsync(TUserMessage message, CancellationToken cancellationToken);

        /// <summary>
        /// 消息确认
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ConfirmMessageByIdAsync(string messageId, CancellationToken cancellationToken);
    }
}
