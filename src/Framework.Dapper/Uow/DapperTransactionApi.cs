using Framework.Uow;
using MySqlConnector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Dapper.Uow
{
    public class DapperTransactionApi : ITransactionApi, ISupportsRollback
    {
        public MySqlTransaction DbTransaction { get; }

        public DapperTransactionApi(MySqlTransaction dbTransaction)
        {
            DbTransaction = dbTransaction;
        }

        public void Dispose()
        {
            try
            {
                DbTransaction.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                return DbTransaction.RollbackAsync();
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                return DbTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                return Task.FromException(ex);
            }
        }
    }
}
