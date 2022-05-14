using Framework.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BlobStoring.FileSystem
{
    public class FileSystemBlobProviderConfiguration
    {
        private readonly BlobContainerConfiguration _containerConfiguration;
        public FileSystemBlobProviderConfiguration(BlobContainerConfiguration containerConfiguration)
        {
            _containerConfiguration = containerConfiguration;
        }

        public bool AppendContainerNameToBasePath
        {
            get => _containerConfiguration.GetConfigurationOrDefault(FileSystemBlobProviderConfigurationNames.AppendContainerNameToBasePath, true);
            set => _containerConfiguration.SetConfiguration(FileSystemBlobProviderConfigurationNames.AppendContainerNameToBasePath, value);
        }

        public string BasePath
        {
            get => _containerConfiguration.GetConfiguration<string>(FileSystemBlobProviderConfigurationNames.BasePath);
            set => _containerConfiguration.SetConfiguration(FileSystemBlobProviderConfigurationNames.BasePath, Check.NotNullOrWhiteSpace(value, nameof(value)));
        }
    }
}
