using System.Collections.Concurrent;

namespace Framework.Security.Users.Session
{

    public interface IUserSession
    {
        void SetOuter(IUserSession outer);
    }
    ///// <summary>
    ///// 用户会话
    ///// </summary>
    //public interface IUserSession<T> where T:class
    //{
    //    /// <summary>
    //    /// 登录
    //    /// </summary>
    //    /// <param name="token"></param>
    //    /// <param name="user"></param>
    //    void Login(string token, T user);

    //    /// <summary>
    //    /// 获取用户信息
    //    /// </summary>
    //    /// <param name="token"></param>
    //    /// <returns></returns>
    //    T GetUser(string token);

    //    /// <summary>
    //    /// 更新登录信息
    //    /// </summary>
    //    /// <param name="token"></param>
    //    /// <param name="user"></param>
    //    void UpdateLogin(string token, T user);
    //}

    ///// <summary>
    ///// 简单的用户会话
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class UserSession<T> : IUserSession<T> where T : class
    //{
    //    public ConcurrentDictionary<string, T> UserSessions { get; private set; } = new ConcurrentDictionary<string, T>();

    //    public T GetUser(string token)
    //    {
    //        if (UserSessions.TryGetValue(token,out T user))
    //        {
    //            return user;
    //        }
    //        return null;
    //    }

    //    public void Login(string token, T user)
    //    {
    //        UserSessions.TryAdd(token, user);
    //    }

    //    public void UpdateLogin(string token, T user)
    //    {
    //        if (UserSessions.TryRemove(token, out T oldUser))
    //        {
    //            UserSessions.TryAdd(token, user);
    //        }
    //    }
    //}

    ///// <summary>
    ///// 调用第三方需要登录的系统时需要给当前用户创建子会话
    ///// </summary>
    ///// <typeparam name="T"></typeparam>
    //public class ChildUserSession<T> : IUserSession<T> where T : class
    //{ 
        
    //}
}
