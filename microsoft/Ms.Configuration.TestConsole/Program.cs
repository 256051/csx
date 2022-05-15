using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Ms.Configuration.FileExtensions;
using Ms.Extensions.Configuration.Json;
using Ms.Extensions.Options;
using Ms.Extensions.Options.ConfigurationExtensions;

namespace Ms.Configuration.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var workDir = $"{Environment.CurrentDirectory}";
            var builder = new ConfigurationBuilder()
              .SetBasePath(workDir)
              .AddJsonFile($"test.json", optional: true, reloadOnChange: true);

            var root = (ConfigurationRoot)builder.Build();
            var services = new ServiceCollection();
            services.Configure<MySqlDbOptions>(root.GetSection("MySqlDbOptions"));
            services.Configure<ChildOptions>(root.GetSection("MySqlDbOptions:ChildOptions"));
            var provider=services.BuildServiceProvider();
            var mySqlDbOptions = provider.GetRequiredService<IOptionsMonitor<MySqlDbOptions>>();
            var childOptions = provider.GetRequiredService<IOptionsMonitor<ChildOptions>>();
            Console.WriteLine($"当前配置值:" + mySqlDbOptions.CurrentValue.ConnectionName+"_"+ childOptions.CurrentValue.Name);
            Console.WriteLine("变更配置,输入任意字符继续");
            Console.ReadLine();
            Console.WriteLine("变更后的配置值" + mySqlDbOptions.CurrentValue.ConnectionName + "_" + childOptions.CurrentValue.Name);
            Console.WriteLine("变更配置,输入任意字符继续");
            Console.ReadLine();
            Console.WriteLine("变更后的配置值" + mySqlDbOptions.CurrentValue.ConnectionName + "_" + childOptions.CurrentValue.Name);
            Console.ReadKey();
        }
    }

    class MySqlDbOptions
    {
        public string ConnectionName { get; set; }

        public List<ChildOptions> Childs { get; set; }

        public IDictionary<string, ChildOptions> Dic { get; set; }
    }

    public class ChildOptions
    { 
        public int Index { get; set; }

        public string Name { get; set; }
    }
}
