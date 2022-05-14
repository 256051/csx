using Framework.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework.BlobStoring
{
    public class BlobContainerFactory : IBlobContainerFactory, ITransient
    {
        protected IBlobProviderSelector ProviderSelector { get; }

        protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

        protected IServiceProvider ServiceProvider { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        public BlobContainerFactory(
            IBlobProviderSelector providerSelector,
            IBlobContainerConfigurationProvider configurationProvider,
            IServiceProvider serviceProvider,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            ProviderSelector = providerSelector;
            ConfigurationProvider = configurationProvider;
            ServiceProvider = serviceProvider;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public IBlobContainer Create(string name)
        {
            var configuration = ConfigurationProvider.Get(name);
            return new BlobContainer(
                name,
                configuration,
                ProviderSelector.Get(name),
                CancellationTokenProvider,
                ServiceProvider
            );
        }
    }
}
