using Framework.IM.Message.Domain;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.IM.Message.Store
{
    public abstract class UserMessageStoreBase<TUserMessage, TKey> :
        IUserMessageStore<TUserMessage>
        where TUserMessage : UserMessage<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        public Task<string> GetContentAsync(TUserMessage userMessage, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(userMessage.Content);
        }

        public Task<string> GetReceiverIdAsync(TUserMessage userMessage, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(userMessage.ReceiverId.ToString());
        }

        public Task<string> GetSenderIdAsync(TUserMessage userMessage, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(userMessage.SenderId.ToString());
        }

        public abstract Task CreateMessageAsync(TUserMessage message, CancellationToken cancellationToken);
        public abstract Task CreateMessagesAsync(IEnumerable<TUserMessage> messages, CancellationToken cancellationToken);
        public abstract Task<TUserMessage> FindByIdAsync(string messageId, CancellationToken cancellationToken);
        public abstract Task UpdateAsync(TUserMessage message, CancellationToken cancellationToken);
        public abstract Task ConfirmMessageByIdAsync(string messageId, CancellationToken cancellationToken);
    }
}
