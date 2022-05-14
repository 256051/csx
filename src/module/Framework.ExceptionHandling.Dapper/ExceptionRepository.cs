using Dapper;
using Dapper.Contrib.Extensions;
using Framework.Core.Data;
using Framework.Core.Dependency;
using Framework.Timing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.ExceptionHandling.Dapper
{
    public class ExceptionRepository : IExceptionRepository, ITransient
    {
        private ILogger<ExceptionRepository> _logger;
        private IDbConnectionProvider _connectionProvider;
        private IClock _clock;
        public ExceptionRepository(IDbConnectionProvider connectionProvider, ILogger<ExceptionRepository> logger, IClock clock)
        {
            _connectionProvider = connectionProvider;
            _logger = logger;
            _clock = clock;
        }

        public async Task<(IEnumerable<ExceptionInfo>, long)> GetPageListAsync(
            int skipCount,
            int resultCount,
            bool? handled,
            int? logLevel,
            DateTime? startTime,
            DateTime? endTime)
        {
            DynamicParameters parameters = new DynamicParameters();
            var where = $" where 1=1 ";
            if (logLevel.HasValue)
            {
                where += " and LogLevel=@LogLevel ";
                parameters.Add("LogLevel", logLevel.Value);
            }
            if (handled.HasValue)
            {
                where += " and Handled=@Handled ";
                parameters.Add("Handled", handled.Value);
            }
            if (startTime.HasValue && endTime.HasValue)
            {
                where += " and CreateTime BETWEEN @StartTime and @EndTime ";
                parameters.Add("StartTime", startTime.Value);
                parameters.Add("EndTime", endTime.Value);
            }
            else
            {
                if (startTime.HasValue)
                {
                    where += " and CreateTime>=@StartTime ";
                    parameters.Add("StartTime", startTime.Value);
                }
                if (endTime.HasValue)
                {
                    where += " and CreateTime<=@EndTime ";
                    parameters.Add("EndTime", endTime.Value);
                }
            }
            var connection =_connectionProvider.Get();
            var entities = await connection.QueryAsync<ExceptionInfo>(
                $"select * from {nameof(ExceptionInfo)} {where} order by CreateTime desc  limit { (skipCount - 1) * resultCount},{resultCount}",
                parameters);
            var count = await connection.QueryFirstAsync<int>($@"select COUNT(1)  from {nameof(ExceptionInfo)} { where}", parameters);
            return (entities, count);
        }

        public async Task InsertAsync(ExceptionInfo exceptionInfo)
        {
            try
            {
                exceptionInfo.CreateTime = _clock.Now;
                await (_connectionProvider.Get()).InsertAsync(exceptionInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError("异常日志添加失败", ex);
            }
        }

        public async Task InsertAsync(IEnumerable<ExceptionInfo> exceptionInfos)
        {
            try
            {
                foreach(var info in exceptionInfos)
                    info.CreateTime = _clock.Now;
                await(_connectionProvider.Get()).InsertAsync(exceptionInfos);
            }
            catch (Exception ex)
            {
                _logger.LogError("异常日志添加失败", ex);
            }
        }
    }
}
