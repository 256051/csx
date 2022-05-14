using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Framework.BlobStoring
{
    public class BlobProviderGetArgs : BlobProviderArgs
    {
        public BlobProviderGetArgs(
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
