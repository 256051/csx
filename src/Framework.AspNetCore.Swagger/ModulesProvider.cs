using Framework.Core;
using Framework.Core.Configurations;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Framework.AspNetCore.Swagger
{
    public class ModulesProvider
    {
        private static List<string> _modules;
        static ModulesProvider()
        {
            _modules = new List<string>();
        }

        public static List<string> GetModules()
        {
            if (_modules.Count == 0)
            {
                var configuration = ApplicationConfiguration.Current;
                foreach (var controller in configuration.Assemblies.SelectMany(s => s.ExportedTypes).Where(w => w.Name.EndsWith("Controller")))
                {
                    _modules.AddIfNotContains(controller
                    .GetCustomAttributes(true)
                    .OfType<ApiExplorerSettingsAttribute>()
                    .Where(w => !string.IsNullOrEmpty(w.GroupName))
                    .Select(s => s.GroupName)
                    .Distinct());
                }
                if (_modules.Count == 0)
                    throw new FrameworkException($"no modules founded in the application swagger config failed");
            }
            return _modules;
        }
    }
}
