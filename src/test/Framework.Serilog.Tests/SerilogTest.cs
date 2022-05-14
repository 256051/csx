using Framework.Test;
using Xunit;

namespace Framework.Serilog.Tests
{
    public class SerilogTest:TestBase
    {
        protected override void LoadModules()
        {
            ApplicationConfiguration.UseSerilog();
        }
    }
}
