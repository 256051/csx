using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Application.Services
{
    public interface IUserInfoProvider
    {
        /// <summary>
        /// 根据角色Id查找用户 调用者的身份认证系统不统一,所以交给调用者自己适配
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<IEnumerable<ImUser>> FindUsersByRoleIdAsync(string roleId,CancellationToken cancellationToken);

        /// <summary>
        /// 根据用户Id查找用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<ImUser> FindByIdAsync(string userId, CancellationToken cancellationToken);
    }
}
