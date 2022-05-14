using Framework.Json;
using Framework.Test;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.RestSharp.Tests
{
    public class RestSharpTest: TestBase
    {
        protected IHttpRestClient _httpRestClient;
        protected IJsonSerializer _jsonSerializer;
        public RestSharpTest()
        {
            _httpRestClient = ServiceProvider.GetRequiredService<IHttpRestClient>();
            _jsonSerializer=ServiceProvider.GetRequiredService<IJsonSerializer>();
        }

        protected override void LoadModules()
        {
            ApplicationConfiguration.UseRestSharp();
        }
    }
}
