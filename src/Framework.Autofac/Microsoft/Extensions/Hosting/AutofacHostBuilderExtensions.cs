using Autofac;
using Framework.Autofac;

namespace Microsoft.Extensions.Hosting
{
    public static class FrameworkAutofacHostBuilderExtensions
    {
        public static IHostBuilder UseAutofac(this IHostBuilder hostBuilder)
        {
            var containerBuilder = new ContainerBuilder();
            return hostBuilder.UseServiceProviderFactory(new FrameworkAutofacServiceProviderFactory(containerBuilder));
        }
    }
}
