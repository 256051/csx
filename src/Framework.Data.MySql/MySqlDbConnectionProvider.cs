using Framework.Core.Data;
using Framework.Core.Dependency;
using MySqlConnector;
using System.Data.Common;

namespace Framework.Data.MySql
{
    public class MySqlDbConnectionProvider : IDbConnectionProvider, ITransient
    {
        private IConnectionStringProvider _connectionStringProvider;
        public MySqlDbConnectionProvider(IConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }

        public DbConnection Get()
        {
            return new MySqlConnection(_connectionStringProvider.GetConnectionString());
        }
    }
}
