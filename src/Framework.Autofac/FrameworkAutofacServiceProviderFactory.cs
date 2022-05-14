using Autofac;
using Autofac.Extensions.DependencyInjection;
using Framework.Core;
using Framework.Security.Users;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Framework.Autofac
{
    public class FrameworkAutofacServiceProviderFactory : IServiceProviderFactory<ContainerBuilder>
    {
        private readonly ContainerBuilder _builder;
        private IServiceCollection _services;

        public FrameworkAutofacServiceProviderFactory(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public ContainerBuilder CreateBuilder(IServiceCollection services)
        {
            _services = services;

            _builder.Populate(services);

            return _builder;
        }

        public IServiceProvider CreateServiceProvider(ContainerBuilder containerBuilder)
        {
            Check.NotNull(containerBuilder, nameof(containerBuilder));
            return new AutofacServiceProvider(containerBuilder.Build());
        }
    }
}
