using System.Collections.Concurrent;

namespace Framework.Expressions
{
    public interface IExpressionsCacheManager
    {
        ConcurrentDictionary<TKey, TResult> GetCache<TKey, TResult>();
    }
}
