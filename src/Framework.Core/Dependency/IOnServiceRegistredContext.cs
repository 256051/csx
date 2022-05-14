using System;

namespace Framework.Core.Dependency
{
    public interface IOnServiceRegistredContext
    {
        Type ImplementationType { get; }
    }
}
