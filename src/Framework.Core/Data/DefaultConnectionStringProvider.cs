using Framework.Core.Dependency;
using System.Threading.Tasks;

namespace Framework.Core.Data
{
    /// <summary>
    /// 默认连接字符来自appsetting配置文件
    /// </summary>
    public class DefaultConnectionStringProvider: IConnectionStringProvider,ISingleton
    {
        public DefaultConnectionStringProvider()
        {

        }

        public string GetConnectionName()
        {
            throw new FrameworkException("please set database connectstring name");
        }

        public string GetConnectionString()
        {
            throw new FrameworkException("please set database connectstring");
        }
    }
}
