using Framework.Core.Dependency;
using System.IO;

namespace Framework.BlobStoring.FileSystem
{
    public class BlobFilePathCalculator : IBlobFilePathCalculator, ITransient
    {
        public string Calculate(BlobProviderArgs args)
        {
            var fileSystemConfiguration = args.Configuration.GetFileSystemConfiguration();
            var blobPath = fileSystemConfiguration.BasePath;
            if (fileSystemConfiguration.AppendContainerNameToBasePath)
            {
                blobPath = Path.Combine(blobPath, args.ContainerName);
            }
            blobPath = Path.Combine(blobPath, args.BlobName);
            return blobPath;
        }
    }
}
