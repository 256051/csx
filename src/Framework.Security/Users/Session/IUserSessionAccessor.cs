namespace Framework.Security.Users.Session
{
    /// <summary>
    /// 用户会话处理
    /// </summary>
    public interface IUserSessionAccessor
    {
        /// <summary>
        /// 当前会话
        /// </summary>
        IUserSession UserSession { get; }

        /// <summary>
        /// 设置当前会话
        /// </summary>
        /// <param name="userSession"></param>
        void SetUserSession(IUserSession userSession);
    }
}
