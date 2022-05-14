using Microsoft.Extensions.DependencyInjection;

namespace Framework.Core.Configurations
{
    /// <summary>
    /// 程序集模块注册约束(注入DI用)
    /// </summary>
    public interface IModuleRegistar
    {
        /// <summary>
        /// 注入程序集到DI
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="assemblies"></param>
        ServiceProvider Register(IServiceCollection collection);
    }
}
