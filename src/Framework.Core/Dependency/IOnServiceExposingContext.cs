using System;
using System.Collections.Generic;

namespace Framework.Core.Dependency
{
    public interface IOnServiceExposingContext
    {
        Type ImplementationType { get; }

        List<Type> ExposedTypes { get; }
    }
}
