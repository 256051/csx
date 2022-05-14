using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Application.Services.Default
{
    public class DefaultUserInfoProvider : IUserInfoProvider
    {
        private IHttpContextAccessor _httpContextAccessor;
        public DefaultUserInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<ImUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(new ImUser()
            {
                UserId = userId,
                UserName = _httpContextAccessor.HttpContext.Request.Query["username"].ToString()
            });
        }

        public Task<IEnumerable<ImUser>> FindUsersByRoleIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return Task.FromResult(Enumerable.Empty<ImUser>());
        }
    }
}
