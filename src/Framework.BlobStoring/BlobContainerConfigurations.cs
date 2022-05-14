using Framework.Core;
using System;
using System.Collections.Generic;

namespace Framework.BlobStoring
{
    public class BlobContainerConfigurations
    {
        private BlobContainerConfiguration Default => GetConfiguration<DefaultContainer>();

        private readonly Dictionary<string, BlobContainerConfiguration> _containers;
        public BlobContainerConfigurations()
        {
            _containers = new Dictionary<string, BlobContainerConfiguration>
            {
                [BlobContainerAttribute.GetContainerName<DefaultContainer>()] = new BlobContainerConfiguration()
            };
        }

        public BlobContainerConfigurations ConfigureDefault(Action<BlobContainerConfiguration> configureAction)
        {
            configureAction(Default);
            return this;
        }


        public BlobContainerConfigurations Configure<TContainer>(
            Action<BlobContainerConfiguration> configureAction)
        {
            return Configure(BlobContainerAttribute.GetContainerName<TContainer>(), configureAction);
        }

        public BlobContainerConfigurations Configure(
            string name,
            Action<BlobContainerConfiguration> configureAction)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNull(configureAction, nameof(configureAction));
            configureAction(
            _containers.GetOrAdd(
                name,
                () => new BlobContainerConfiguration(Default)
                )
            );
            return this;
        }

        public BlobContainerConfigurations ConfigureAll(Action<string, BlobContainerConfiguration> configureAction)
        {
            foreach (var container in _containers)
            {
                configureAction(container.Key, container.Value);
            }
            return this;
        }

        public BlobContainerConfiguration GetConfiguration<TContainer>()
        {
            return GetConfiguration(BlobContainerAttribute.GetContainerName<TContainer>());
        }

        public BlobContainerConfiguration GetConfiguration(string name)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            return _containers.GetOrDefault(name)?? throw new FrameworkException($"the key named {name} does not have a BlobContainerConfiguration"); ;
        }
    }
}
