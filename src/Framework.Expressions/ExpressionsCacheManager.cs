using Framework.Core.Dependency;
using System.Collections.Concurrent;

namespace Framework.Expressions
{
    public class ExpressionsCacheManager: IExpressionsCacheManager,ISingleton
    {
        private IExpressionsCacheProvider _expressionsCacheProvider;

        public ExpressionsCacheManager(IExpressionsCacheProvider expressionsCacheProvider)
        {
            _expressionsCacheProvider = expressionsCacheProvider;
        }

        public ConcurrentDictionary<TKey, TResult> GetCache<TKey, TResult>()
        {
            return _expressionsCacheProvider.GetPropertySetValueCached<TKey, TResult>();
        }


    }
}
