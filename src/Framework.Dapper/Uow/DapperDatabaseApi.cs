using Framework.Uow;
using MySqlConnector;

namespace Framework.Dapper.Uow
{
    public class DapperDatabaseApi : IDatabaseApi
    {
        public MySqlConnection DbConnection { get; }

        public DapperDatabaseApi(MySqlConnection _dbConnection)
        {
            DbConnection = _dbConnection;
        }

        public void Dispose()
        {
            DbConnection.Close();
            DbConnection.Dispose();
        }
    }
}
