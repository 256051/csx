using Framework.Core.Dependency;
using Microsoft.Extensions.Options;

namespace Framework.BlobStoring
{
    public class BlobContainerConfigurationProvider : IBlobContainerConfigurationProvider, ITransient
    {
        protected BlobStoringOptions Options { get; }

        public BlobContainerConfigurationProvider(IOptions<BlobStoringOptions> options)
        {
            Options = options.Value;
        }

        public BlobContainerConfiguration Get(string name)
        {
            return Options.Containers.GetConfiguration(name);
        }
    }
}
