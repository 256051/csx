using Framework.Core.Configurations;
using Framework.Test;

namespace Framework.Word.Npoi.Tests
{
    public class NpoiTest: TestBase
    {
        protected override void LoadModules()
        {
            ApplicationConfiguration
                .UseConfiguration()
                .UseNpoiWord();
        }
    }
}
