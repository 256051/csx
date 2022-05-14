using Framework.Core;
using System.Threading;

namespace Framework.BlobStoring
{
    public abstract class BlobProviderArgs
    {
        public string ContainerName { get; }

        public BlobContainerConfiguration Configuration { get; }

        public string BlobName { get; }

        public CancellationToken CancellationToken { get; }

        protected BlobProviderArgs(
            string containerName,
            BlobContainerConfiguration configuration,
            string blobName,
            CancellationToken cancellationToken = default)
        {
            ContainerName = Check.NotNullOrWhiteSpace(containerName, nameof(containerName));
            Configuration = Check.NotNull(configuration, nameof(configuration));
            BlobName = Check.NotNullOrWhiteSpace(blobName, nameof(blobName));
            CancellationToken = cancellationToken;
        }
    }
}
