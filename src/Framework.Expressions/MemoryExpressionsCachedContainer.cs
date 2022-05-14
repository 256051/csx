using System.Collections.Concurrent;

namespace Framework.Expressions
{
    /// <summary>
    /// 内存表达式树缓存容器
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TObject"></typeparam>
    public class MemoryExpressionsCachedContainer<TKey, TObject>
    {
        private static ConcurrentDictionary<string, ConcurrentDictionary<TKey, TObject>> _container = new ConcurrentDictionary<string, ConcurrentDictionary<TKey, TObject>>();

        public static ConcurrentDictionary<TKey, TObject> GetOrAdd(string key)
        {
            return _container.GetOrAdd(key, value =>
            {
                return new ConcurrentDictionary<TKey, TObject>();
            });
        }
    }
}
