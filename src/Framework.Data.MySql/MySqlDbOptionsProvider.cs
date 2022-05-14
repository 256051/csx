using Framework.Core;
using Framework.Core.Dependency;
using Microsoft.Extensions.Options;

namespace Framework.Data.MySql
{
    public class MySqlDbOptionsProvider:ISingleton
    {
        public const string ConfigurationKey = "MySqlDbOptions";
        private MySqlDbOptions _mySqlDbOptions;
        public MySqlDbOptionsProvider(IOptions<MySqlDbOptions> options)
        {
            _mySqlDbOptions = options.Value;
        }

        public MySqlDbOptions Get()
        {
            if (_mySqlDbOptions == null)
                throw new FrameworkException($"please set {ConfigurationKey} in appsettings.json because mysql module need this options to connect mysql database server");
            return _mySqlDbOptions;
        }
    }

}
