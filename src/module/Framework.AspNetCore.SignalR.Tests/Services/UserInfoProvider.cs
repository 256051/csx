using Dapper;
using Framework.AspNetCore.SignalR.Application;
using Framework.AspNetCore.SignalR.Application.Services;
using Framework.Core.Data;
using Framework.Core.Dependency;
using Framework.Uow;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Tests.Services
{
    public class UserInfoProvider : IUserInfoProvider, IReplace
    {
        private IUnitOfWorkManager _unitOfWorkManager;
        private IDbProvider _dbProvider;

        public UserInfoProvider(IUnitOfWorkManager unitOfWorkManager, IDbProvider dbProvider)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _dbProvider = dbProvider;
        }

        public async Task<ImUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions()))
            {
                var sql = @"SELECT
                            	id as UserId,real_name as UserName
                            FROM
                            	ac_user 
                            	WHERE id=@userId ";
                var connection = await _dbProvider.GetConnectionAsync();
                return await connection.QueryFirstOrDefaultAsync<ImUser>(sql, new { userId }, await _dbProvider.GetTransactionAsync());
            }
        }

        /// <summary>
        /// 留置平台的角色体系设计缺陷导致这边很难扩展,所以采用用户对用户发送消息的模式
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ImUser>> FindUsersByRoleIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions()))
            {
                var sql = @"SELECT
                            	_user.id as UserId 
                            FROM
                            	ac_user _user
                            	INNER JOIN ac_role _role ON _user.role_id = _role.id
                            	WHERE _role.id=@roleId ";
                return await (await _dbProvider.GetConnectionAsync()).QueryAsync<ImUser>(sql, new { roleId }, await _dbProvider.GetTransactionAsync());
            }
        }
    }
}
