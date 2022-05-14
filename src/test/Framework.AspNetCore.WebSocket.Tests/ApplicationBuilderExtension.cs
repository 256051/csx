using Framework.AspNetCore.WebSocket.Messages;
using Framework.AspNetCore.WebSocket.Tests.Entities;
using Framework.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using WPSM.Data.Mq.Domain;

namespace Framework.AspNetCore.WebSocket.Tests
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseApplication(this IApplicationBuilder builder)
        {
            var services = builder.ApplicationServices;
            var jsonSerializer = services.GetRequiredService<IJsonSerializer>();
          
            var localManager= services.GetRequiredService<WPSMDataMqLocalManager<WPSMDataMQLocalInfo>>();
            localManager.LocalSubscribeHeartRateStore(heartInfo=> {
                var logger = services.GetRequiredService<ILogger<WPSMDataMqLocalManager<WPSMDataMQLocalInfo>>>();
                var provider=services.GetRequiredService<IEnumerable<MessageProvider>>().Where(provider=>provider.RoutePath==Consts.Heart).FirstOrDefault();
                var message = jsonSerializer.Serialize(heartInfo);
                provider.PushMessage(message);
                logger.LogInformation(message);
            }, default);

            localManager.LocalSubscribeBreatheStore(breath =>
            {
                var provider = services.GetRequiredService<IEnumerable<MessageProvider>>().Where(provider => provider.RoutePath == Consts.Breath).FirstOrDefault();
                provider.PushMessage(jsonSerializer.Serialize(breath));
            }, default);
            return builder;
        }

      
    }
}
