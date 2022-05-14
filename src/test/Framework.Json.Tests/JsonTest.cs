using Framework.Json.SystemTextJson;
using Framework.Test;
using Framework.Timing;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Json.Tests
{
    public class JsonTest: TestBase
    {
        protected IJsonSerializer _jsonSerializer;

        public JsonTest()
        {
            _jsonSerializer = ServiceProvider.GetRequiredService<IJsonSerializer>();
        }

        protected override void LoadModules()
        {
            //Services.Configure<SystemTextJsonSerializerOptions>(options =>
            //{
            //    options.UnsupportedTypes.Add<Person>();
            //});

            Services.Configure<JsonOptions>(options =>
            {
                options.DefaultDateTimeFormat = DateTimeFormats.LongTime;
            });
        }
    }
}
