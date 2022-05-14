using Framework.Core.Dependency;
using System.Collections.Concurrent;

namespace Framework.Expressions
{
    /// <summary>
    /// 内存
    /// </summary>
    public class MemoryExpressionsCacheProvider : IExpressionsCacheProvider,ISingleton
    {
        private const string _key = "memoryexpressionscache";
        public ConcurrentDictionary<TKey, TResult> GetPropertySetValueCached<TKey, TResult>()
        {
            return MemoryExpressionsCachedContainer<TKey, TResult>.GetOrAdd(_key);
        }
    }
}
