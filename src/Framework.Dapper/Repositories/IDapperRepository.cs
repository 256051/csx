using System.Data;
using System.Threading.Tasks;

namespace Framework.Dapper.Repositories
{
    public interface IDapperRepository
    {
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        Task<IDbConnection> GetDbConnectionAsync();

        /// <summary>
        /// 获取数据库事务
        /// </summary>
        /// <returns></returns>
        Task<IDbTransaction> GetDbTransactionAsync();
    }
}
