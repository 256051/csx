using Framework.Test;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Framework.Logging.Tests
{
    public class LoggingTest : TestBase
    {
        protected ILogger<LoggingTest> Logger;
        public LoggingTest()
        {
            Logger = ServiceProvider.GetRequiredService<ILogger<LoggingTest>>();
        }

        protected override void LoadModules()
        {
            //ÒýÈëdebug ·½±ã²âÊÔ
            Services.AddLogging(builder =>
            {
                builder.AddDebug();
            });
        }
    }
}
