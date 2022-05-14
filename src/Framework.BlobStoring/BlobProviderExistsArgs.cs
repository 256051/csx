using System.Threading;

namespace Framework.BlobStoring
{
    /// <summary>
    /// 判断是否存在
    /// </summary>
    public class BlobProviderExistsArgs : BlobProviderArgs
    {
        public BlobProviderExistsArgs(
            string containerName,
            BlobContainerConfiguration configuration,
            string blobName,
            CancellationToken cancellationToken = default)
        : base(
            containerName,
            configuration,
            blobName,
            cancellationToken)
        {
        }
    }
}
