using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Framework.Serilog.Tests
{
    public class LogTest: SerilogTest
    {
        private ILogger<LogTest> _logger;
        public LogTest()
        {
            _logger = ApplicationConfiguration.Provider.GetRequiredService<ILogger<LogTest>>();
        }

        [Fact]
        public void WriteTest()
        {
            for (var i = 0; i < 100000; i++)
            {
                _logger.LogInformation("LogInformation Test");
                _logger.LogError("LogError Test");
                _logger.LogDebug("LogDebug Test");
                _logger.LogWarning("LogDebug Test");
                _logger.LogTrace("LogTrace Test");
                _logger.LogCritical("LogCritical Test");
            }
        }
    }
}
