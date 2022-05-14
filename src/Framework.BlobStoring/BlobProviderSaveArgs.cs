using Framework.Core;
using System.IO;
using System.Threading;

namespace Framework.BlobStoring
{
    /// <summary>
    /// 保存参数
    /// </summary>
    public class BlobProviderSaveArgs:BlobProviderArgs
    {
        /// <summary>
        /// 二进制块对象
        /// </summary>
        public Stream BlobStream { get; }

        /// <summary>
        /// 覆盖已存在的
        /// </summary>
        public bool OverrideExisting { get; }

        public BlobProviderSaveArgs(
            string containerName,
            BlobContainerConfiguration configuration,
            string blobName,
            Stream blobStream,
            bool overrideExisting = false,
            CancellationToken cancellationToken = default)
            : base(
                containerName,
                configuration,
                blobName,
                cancellationToken)
        {
            BlobStream = Check.NotNull(blobStream, nameof(blobStream));
            OverrideExisting = overrideExisting;
        }
    }
}
