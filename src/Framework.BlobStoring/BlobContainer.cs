using Framework.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.BlobStoring
{
    public class BlobContainer<TContainer> : IBlobContainer<TContainer> where TContainer : class
    {
        private readonly IBlobContainer _container;

        public BlobContainer(IBlobContainerFactory blobContainerFactory)
        {
            _container = blobContainerFactory.Create<TContainer>();
        }

        public Task SaveAsync(
        string name,
        Stream stream,
        bool overrideExisting = false,
        CancellationToken cancellationToken = default)
        {
            return _container.SaveAsync(
                name,
                stream,
                overrideExisting,
                cancellationToken
            );
        }

        public Task<bool> DeleteAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.DeleteAsync(
                name,
                cancellationToken
            );
        }

        public Task<bool> ExistsAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.ExistsAsync(
                name,
                cancellationToken
            );
        }

        public Task<Stream> GetAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.GetAsync(
                name,
                cancellationToken
            );
        }

        public Task<Stream> GetOrNullAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            return _container.GetOrNullAsync(
                name,
                cancellationToken
            );
        }
    }


    public class BlobContainer : IBlobContainer
    {
        public string ContainerName { get; }

        protected BlobContainerConfiguration Configuration { get; }

        protected IBlobProvider Provider { get; }

        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        protected IServiceProvider ServiceProvider { get; }

        public BlobContainer(
            string containerName,
            BlobContainerConfiguration configuration,
            IBlobProvider provider,
            ICancellationTokenProvider cancellationTokenProvider,
            IServiceProvider serviceProvider)
        {
            ContainerName = containerName;
            Configuration = configuration;
            Provider = provider;
            CancellationTokenProvider = cancellationTokenProvider;
            ServiceProvider = serviceProvider;
        }

        public async Task SaveAsync(string name, Stream stream, bool overrideExisting = false, CancellationToken cancellationToken = default)
        {
            var (normalizedContainerName, normalizedBlobName) = NormalizeNaming(ContainerName, name);

            await Provider.SaveAsync(
                new BlobProviderSaveArgs(
                    normalizedContainerName,
                    Configuration,
                    normalizedBlobName,
                    stream,
                    overrideExisting,
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }

        public async Task<bool> DeleteAsync(string name, CancellationToken cancellationToken = default)
        {
            var (normalizedContainerName, normalizedBlobName) =
                    NormalizeNaming(ContainerName, name);

            return await Provider.DeleteAsync(
                new BlobProviderDeleteArgs(
                    normalizedContainerName,
                    Configuration,
                    normalizedBlobName,
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }

        public async Task<bool> ExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            var (normalizedContainerName, normalizedBlobName) =
                    NormalizeNaming(ContainerName, name);

            return await Provider.ExistsAsync(
                new BlobProviderExistsArgs(
                    normalizedContainerName,
                    Configuration,
                    normalizedBlobName,
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }

        public async Task<Stream> GetAsync(string name, CancellationToken cancellationToken = default)
        {
            var stream = await GetOrNullAsync(name, cancellationToken);

            if (stream == null)
            {
                throw new FrameworkException(
                    $"Could not found the requested BLOB '{name}' in the container '{ContainerName}'!");
            }

            return stream;
        }

        public async Task<Stream> GetOrNullAsync(string name, CancellationToken cancellationToken = default)
        {
            var (normalizedContainerName, normalizedBlobName) =
                    NormalizeNaming(ContainerName, name);

            return await Provider.GetOrNullAsync(
                new BlobProviderGetArgs(
                    normalizedContainerName,
                    Configuration,
                    normalizedBlobName,
                    CancellationTokenProvider.FallbackToProvider(cancellationToken)
                )
            );
        }

        protected virtual (string, string) NormalizeNaming(string containerName, string blobName)
        {
            if (!Configuration.NamingNormalizers.Any())
            {
                return (containerName, blobName);
            }

            using (var scope = ServiceProvider.CreateScope())
            {
                foreach (var normalizerType in Configuration.NamingNormalizers)
                {
                    var normalizer = scope.ServiceProvider
                        .GetRequiredService(normalizerType)
                        .As<IBlobNamingNormalizer>();

                    containerName = normalizer.NormalizeContainerName(containerName);
                    blobName = normalizer.NormalizeBlobName(blobName);
                }

                return (containerName, blobName);
            }
        }
    }
}
