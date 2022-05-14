using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.IM.Message.Domain
{
    public class UserMessageManager<TUserMessage> where TUserMessage : class
    {
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        protected IUserMessageStore<TUserMessage> Store { get; set; }

        public UserMessageManager(IUserMessageStore<TUserMessage> store)
        {
            Store = store;
        }

        /// <summary>
        /// 发送单挑信息
        /// </summary>
        /// <param name="userMessage"></param>
        /// <returns></returns>
        public async Task CreateMessageAsync(TUserMessage userMessage)
        {
            if(await IsValidMessage(userMessage))
            await Store.CreateMessageAsync(userMessage, CancellationToken);
        }

        /// <summary>
        /// 批量发送消息
        /// </summary>
        /// <param name="messages"></param>
        /// <returns></returns>
        public async Task CreateMessagesAsync(IEnumerable<TUserMessage> messages)
        {
            var validMessages=messages.Where(message => IsValidMessage(message).ConfigureAwait(false).GetAwaiter().GetResult());
            await Store.CreateMessagesAsync(validMessages, CancellationToken);
        }

        /// <summary>
        /// 确认收到消息
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        public async Task ConfirmMessageByIdAsync(string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
                throw new ArgumentNullException(nameof(messageId));
            var existedMessage = await Store.FindByIdAsync(messageId,CancellationToken);
            if (existedMessage==null)
                throw new Exception($"can not find message by id:{messageId}");
            await Store.ConfirmMessageByIdAsync(messageId, CancellationToken);
        }

        public Task<TUserMessage> FindByIdAsync(string messageId)
        {
            if (string.IsNullOrEmpty(messageId))
                throw new ArgumentNullException(nameof(messageId));
            return Store.FindByIdAsync(messageId, CancellationToken);
        }

        private async Task<bool> IsValidMessage(TUserMessage userMessage)
        {
            if (string.IsNullOrEmpty(await Store.GetContentAsync(userMessage, CancellationToken)))
            {
                return false;
            }
            if (string.IsNullOrEmpty(await Store.GetReceiverIdAsync(userMessage, CancellationToken)))
            {
                return false;
            }
            if (string.IsNullOrEmpty(await Store.GetSenderIdAsync(userMessage, CancellationToken)))
            {
                return false;
            }
            return true;
        }
    }
}
