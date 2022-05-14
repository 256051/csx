namespace Framework.Security.Users.Session
{
    /// <summary>
    /// 用户会话管理
    /// </summary>
    public interface IUserSessionManager
    {
        /// <summary>
        /// 当前会话
        /// </summary>
        IUserSession Current { get; }

        /// <summary>
        /// 开启用户会话
        /// </summary>
        /// <returns></returns>
        IUserSession Begin();
    }
}
