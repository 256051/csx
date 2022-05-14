using Framework.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Framework.Core.Configurations
{
    public class ModuleRegistar : IModuleRegistar
    {
        public ServiceProvider Register(IServiceCollection collection)
        {
            var assemblies = ApplicationConfiguration.Current.GetModules();
            foreach (var assembly in assemblies)
            {
                var types = assembly
                    .GetTypes()
                    .Where(type => type != null &&
                            type.IsClass &&
                            !type.IsAbstract &&
                            !type.IsGenericType).ToArray(); 

                foreach (var type in types)
                {
                    foreach (var registrar in collection.GetConventionalRegistrars())
                    {
                        registrar.AddType(collection, type);
                    }
                }
            }
            var builder= collection.BuildServiceProvider();
            collection.AddSingleton<IServiceProvider>(builder);
            collection.AddSingleton(builder);
            return builder;
        }
    }
}
