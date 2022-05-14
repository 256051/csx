using Dapper;
using Framework.AspNetCore.SignalR.Application.Models;
using Framework.Core.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Application
{
    [ApiController]
    [Route("[controller]")]
    public class ImMessageController : ControllerBase
    {
        private IDbProvider _dbProvider;
        public ImMessageController(IDbProvider dbProvider)
        {
            _dbProvider = dbProvider;
        }

        /// <summary>
        /// 获取用户的收件列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("receivinglist")]
        public async Task<IEnumerable<MessageQueryResultDto>> GetReceivingListAsync(MessageQueryDto dto)
        {
            var connection = await _dbProvider.GetConnectionAsync();
            var sql = @"SELECT Id,Content as Message,CreateTime as SendTime,Confirmed FROM imusermessage 
                       WHERE ReceiverId = @UserId";
            if (dto.StartTime.HasValue)
            {
                sql += " And CreateTime>=@StartTime ";
            }
            if (dto.EndTime.HasValue)
            {
                sql += " And CreateTime<=@EndTime ";
            }
            sql += " ORDER BY CreateTime DESC";
            return await connection.QueryAsync<MessageQueryResultDto>(sql, dto, await _dbProvider.GetTransactionAsync());
        }
    }
}
