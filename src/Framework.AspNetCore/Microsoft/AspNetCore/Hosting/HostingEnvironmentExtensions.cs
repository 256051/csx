using System;
using Framework.Core.Configurations;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Hosting
{
    public static class HostingEnvironmentExtensions
    {
        public static void BuildConfiguration(
            this IWebHostEnvironment env,
            ConfigurationBuilderOptions options = null)
        {
            options = options ?? new ConfigurationBuilderOptions();

            if (string.IsNullOrEmpty(options.BasePath))
            {
                options.BasePath = env.ContentRootPath;
            }

            if (string.IsNullOrEmpty(options.EnvironmentName))
            {
                options.EnvironmentName = env.EnvironmentName;
            }
            ConfigurationHelper.BuildConfiguration(options);
        }
    }
}
