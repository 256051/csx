using Framework.Core.Data;
using System.Data;
using System.Threading.Tasks;

namespace Framework.Dapper.Repositories
{
    public class DapperRepository : IDapperRepository
    {
        private IDbProvider _dbProvider;
        public DapperRepository(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        public async Task<IDbConnection> GetDbConnectionAsync()
        {
            return await _dbProvider.GetConnectionAsync();
        }

        public async Task<IDbTransaction> GetDbTransactionAsync()
        {
            return await _dbProvider.GetTransactionAsync();
        }
    }
}
