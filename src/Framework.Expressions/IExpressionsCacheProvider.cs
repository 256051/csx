using System.Collections.Concurrent;

namespace Framework.Expressions
{
    /// <summary>
    /// 表达式树缓存Provider
    /// </summary>
    public interface IExpressionsCacheProvider
    {
        /// <summary>
        /// 获取属性赋值的表达式树缓存块
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        ConcurrentDictionary<TKey, TResult> GetPropertySetValueCached<TKey, TResult>();
    }
}
