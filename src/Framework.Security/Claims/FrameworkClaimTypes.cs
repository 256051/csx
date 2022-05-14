using System.Security.Claims;

namespace Framework.Security.Claims
{
    public static class FrameworkClaimTypes
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName { get; set; } = ClaimTypes.Name;

        /// <summary>
        /// 用户Id
        /// </summary>
        public static string UserId { get; set; } = ClaimTypes.Sid;

        /// <summary>
        /// 角色
        /// </summary>
        public static string Role { get; set; } = ClaimTypes.Role;
    }
}
