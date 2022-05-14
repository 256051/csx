using Framework.Core;
using Framework.Core.Dependency;
using Framework.Core.DynamicProxy;
using System.Collections.Generic;
using System.Linq;

namespace Framework.BlobStoring
{
    public class BlobProviderSelector : IBlobProviderSelector, ITransient
    {
        protected IEnumerable<IBlobProvider> BlobProviders { get; }

        protected IBlobContainerConfigurationProvider ConfigurationProvider { get; }

        public BlobProviderSelector(
            IBlobContainerConfigurationProvider configurationProvider,
            IEnumerable<IBlobProvider> blobProviders)
        {
            ConfigurationProvider = configurationProvider;
            BlobProviders = blobProviders;
        }


        public IBlobProvider Get(string containerName)
        {
            Check.NotNull(containerName, nameof(containerName));

            var configuration = ConfigurationProvider.Get(containerName);

            if (!BlobProviders.Any())
            {
                throw new FrameworkException("No BLOB Storage provider was registered! At least one provider must be registered to be able to use the Blog Storing System.");
            }

            foreach (var provider in BlobProviders)
            {
                if (configuration.ProviderType.IsAssignableFrom(ProxyHelper.GetUnProxiedType(provider)))
                {
                    return provider;
                }
            }

            throw new FrameworkException(
                $"Could not find the BLOB Storage provider with the type ({configuration.ProviderType.AssemblyQualifiedName}) configured for the container {containerName} and no default provider was set."
            );
        }
    }
}
