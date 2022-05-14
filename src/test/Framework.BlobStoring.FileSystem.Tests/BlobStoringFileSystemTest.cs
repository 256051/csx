using Framework.Core.Dependency;
using Framework.Test;
using Microsoft.Extensions.Configuration;

namespace Framework.BlobStoring.FileSystem.Tests
{
    public class BlobStoringFileSystemTest : TestBase
    {
        public BlobStoringFileSystemTest()
        {
            
        }

        protected override void LoadModules()
        {

            //ApplicationConfiguration.Container.ReplaceConfiguration(ConfigurationHelper.BuildConfiguration(builderAction: builder =>
            //{
            //    builder.AddJsonFile(, optional: true, reloadOnChange: true);
            //}));

            ApplicationConfiguration
                .UseBlobStoring()
                .UseFileSystemBlobStoring();

        }
    }
}
