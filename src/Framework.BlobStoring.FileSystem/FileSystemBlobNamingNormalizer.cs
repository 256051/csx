using Framework.Core.Dependency;
using Framework.Core.Localization;
using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Framework.BlobStoring.FileSystem
{
    public class FileSystemBlobNamingNormalizer : IBlobNamingNormalizer, ITransient
    {
        private readonly IOSPlatformProvider _iosPlatformProvider;

        public FileSystemBlobNamingNormalizer(IOSPlatformProvider iosPlatformProvider)
        {
            _iosPlatformProvider = iosPlatformProvider;
        }

        public virtual string NormalizeContainerName(string containerName)
        {
            return Normalize(containerName);
        }

        public virtual string NormalizeBlobName(string blobName)
        {
            return Normalize(blobName);
        }

        protected virtual string Normalize(string fileName)
        {
            using (CultureHelper.Use(CultureInfo.InvariantCulture))
            {
                var os = _iosPlatformProvider.GetCurrentOSPlatform();
                if (os == OSPlatform.Windows)
                {
                    fileName = Regex.Replace(fileName, "[:\\*\\?\"<>\\|]", string.Empty);
                }
                return fileName;
            }
        }
    }
}
