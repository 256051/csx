using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.Core.Dependency
{
    public interface IConventionalRegistrar
    {
        void AddType(IServiceCollection services, Type type);
    }
}
