using Microsoft.Extensions.Configuration;
using System;

namespace Microsoft.Configuration.Json.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var workDir = $"{Environment.CurrentDirectory}";
            var builder = new ConfigurationBuilder()
              .SetBasePath(workDir)
              .AddJsonFile($"test.json", optional: true, reloadOnChange: true);

            var root = builder.Build();
            Console.WriteLine($"当前配置值:"+root["MySqlDbOptions:ConnectionName"]);
            Console.WriteLine("变更配置,输入任意字符继续");
            Console.ReadLine();
            Console.WriteLine("变更后的配置值"+root["MySqlDbOptions:ConnectionName"]);
            Console.ReadKey();
        }
    }
}
