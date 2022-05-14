using Dapper;
using Dapper.Contrib.Extensions;
using Framework.Core.Data;
using Framework.IM.Message.Store;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.IM.Message.Dapper
{
    public class UserMessageStore<TUserMessage, TKey> : UserMessageStoreBase< TUserMessage, TKey>
        where TUserMessage : UserMessage<TKey>, new()
        where TKey:IEquatable<TKey>
    {

        protected IDbProvider DbProvider;
        public UserMessageStore(IDbProvider dbProvider)
        {
            DbProvider = dbProvider;
        }

        public override async Task ConfirmMessageByIdAsync(string messageId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var connection = await DbProvider.GetConnectionAsync();
            await connection.ExecuteAsync("update ImUserMessage set Confirmed=1,ConfirmedTime=@confirmedTime where Id=@messageId", new { messageId, confirmedTime=DateTime.Now }, await DbProvider.GetTransactionAsync());
        }

        public override async Task CreateMessageAsync(TUserMessage message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var connection = await DbProvider.GetConnectionAsync();
            await connection.InsertAsync(message, await DbProvider.GetTransactionAsync());
        }

        public override async Task CreateMessagesAsync(IEnumerable<TUserMessage> messages, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var connection = await DbProvider.GetConnectionAsync();
            await connection.InsertAsync(messages, await DbProvider.GetTransactionAsync());
        }

        public override async Task<TUserMessage> FindByIdAsync(string messageId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var connection = await DbProvider.GetConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<TUserMessage>("select * from ImUserMessage where Id=@messageId",new { messageId }, await DbProvider.GetTransactionAsync());
        }

        public override async Task UpdateAsync(TUserMessage message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var connection = await DbProvider.GetConnectionAsync();
            await connection.UpdateAsync(message, await DbProvider.GetTransactionAsync());
        }
    }
}
