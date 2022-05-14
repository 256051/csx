using Framework.Core.Dependency;

namespace Framework.Security.Users
{
    public interface ICurrentUser
    {
        /// <summary>
        /// 是否通过认证
        /// </summary>
        bool IsAuthenticated { get;  }

        /// <summary>
        /// 用户id
        /// </summary>
        string Id { get; }

        /// <summary>
        /// 用户名
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 角色id
        /// </summary>
        string RoleId { get;  }
    }
}
