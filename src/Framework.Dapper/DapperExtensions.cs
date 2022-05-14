using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Framework.Dapper
{
    /// <summary>
    /// Dapper扩展
    /// </summary>
    public static class DapperExtensions
    {
        /// <summary>
        /// 分页查询(支持多表)
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="connection"></param>
        /// <param name="skipCount">第几页</param>
        /// <param name="resultCount">每页显示记录数</param>
        /// <param name="returnFields">返回的字段</param>
        /// <param name="where">查询条件(包括连接语句)</param>
        /// <param name="param">查询参数绑定的实体(需要掌握Dapper参数化查询用法)</param>
        /// <param name="orderBy">排序语句</param>
        /// <returns></returns>
        public static async Task<(IEnumerable<TEntity>, long)> GetListByPageAsync<TEntity>(this IDbConnection connection, string returnFields, string where, int skipCount = 1, int resultCount = 10, object param = null, string orderBy = null, IDbTransaction transaction = null)
        {
            int skip = 0;
            if (skipCount > 0)
            {
                skip = (skipCount - 1) * resultCount;
            }
            var count = await connection.QueryFirstAsync<int>($"SELECT COUNT(1) FROM {where}",param, transaction);
            var entities = await connection.QueryAsync<TEntity>($"SELECT {returnFields} FROM {where} order by {orderBy}  LIMIT {skip},{resultCount}", param, transaction);
            return (entities, count);
        }
    }
}
