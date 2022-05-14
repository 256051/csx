using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.ExceptionHandling.Dapper
{
    public interface IExceptionRepository
    {
        /// <summary>
        /// 异常信息列表
        /// </summary>
        /// <param name="skipCount"></param>
        /// <param name="resultCount"></param>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<(IEnumerable<ExceptionInfo>, long)> GetPageListAsync(
            int skipCount,
            int resultCount,
            bool? handled,
            int? logLevel,
            DateTime? startTime,
            DateTime? endTime);

        /// <summary>
        /// 插入一条异常
        /// </summary>
        /// <param name="exceptionInfo"></param>
        /// <returns></returns>
        Task InsertAsync(ExceptionInfo exceptionInfo);

        /// <summary>
        /// 插入异常集合
        /// </summary>
        /// <param name="exceptionInfo"></param>
        /// <returns></returns>
        Task InsertAsync(IEnumerable<ExceptionInfo> exceptionInfos);
    }
}
