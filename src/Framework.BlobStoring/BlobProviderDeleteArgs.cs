using System.Threading;

namespace Framework.BlobStoring
{
    /// <summary>
    /// 删除参数
    /// </summary>
    public class BlobProviderDeleteArgs : BlobProviderArgs
    {
        public BlobProviderDeleteArgs(
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
