using Autofac.Core.Activators.Reflection;
using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace Framework.Autofac
{
    public class AutofacConstructorFinder : IConstructorFinder
    {
        private const BindingFlags DeclaredOnlyPublicFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly; //Remove static constructor, BindingFlags.Static

        private readonly Func<Type, ConstructorInfo[]> _finder;

        private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> DefaultPublicConstructorsCache = new ConcurrentDictionary<Type, ConstructorInfo[]>();

        public AutofacConstructorFinder()
            : this(GetDefaultPublicConstructors)
        {
        }

        public AutofacConstructorFinder(Func<Type, ConstructorInfo[]> finder)
        {
            _finder = finder ?? throw new ArgumentNullException(nameof(finder));
        }

        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            return _finder(targetType);
        }

        private static ConstructorInfo[] GetDefaultPublicConstructors(Type type)
        {
            var retval = DefaultPublicConstructorsCache.GetOrAdd(type, t => t.GetConstructors(DeclaredOnlyPublicFlags));

            if (retval.Length == 0)
            {
                throw new NoConstructorsFoundException(type);
            }

            return retval;
        }
    }
}
