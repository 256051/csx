using Framework.Test;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.RabbitMQ.Client.Tests
{
    public class RabbitClientTest : TestBase
    {
        protected IRabbmitClient _rabbmitClient;
        public RabbitClientTest()
        {
            _rabbmitClient = ServiceProvider.GetRequiredService<IRabbmitClient>(); ;
        }

        protected override void LoadModules()
        {
            ApplicationConfiguration.UseRabbitClient();
        }
    }
}
