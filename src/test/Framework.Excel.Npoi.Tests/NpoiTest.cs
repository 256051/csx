using Framework.Core.Configurations;
using Framework.Test;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Excel.Npoi.Tests
{
    public class NpoiTest: TestBase
    {
        protected override void LoadModules()
        {
            ApplicationConfiguration
                .UseConfiguration()
                .UseNpoiExcel(options => {
                    options.ConfigFile = "excelsettings.json";
                });
        }
    }
}
