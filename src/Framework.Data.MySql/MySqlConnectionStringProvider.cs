using Framework.Core;
using Framework.Core.Data;
using Framework.Core.Dependency;

namespace Framework.Data.MySql
{
    public class MySqlConnectionStringProvider : IConnectionStringProvider,ITransient
    {
        private MySqlDbOptionsProvider _mySqlDbOptionsProvider;

        public MySqlConnectionStringProvider(MySqlDbOptionsProvider mySqlDbOptionsProvider)
        {
            _mySqlDbOptionsProvider = mySqlDbOptionsProvider;
        }

        public string GetConnectionName()
        {
            var connectionName = _mySqlDbOptionsProvider.Get()?.ConnectionName;
            if (string.IsNullOrEmpty(connectionName))
            {
                throw new FrameworkException("MySqlDbOptions configuration node not found ConnectionName in appseeting.json ,please set it");
            }
            return connectionName;
        }

        public string GetConnectionString()
        {
            var connectionString = _mySqlDbOptionsProvider.Get()?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new FrameworkException("MySqlDbOptions configuration node not found ConnectionString in appseeting.json ,please set it");
            }
            return connectionString;
        }
    }
}