using System;

namespace Framework.Core.Dependency
{
    /// <summary>
    /// ServiceProvider缓存类型接口,第一次释出类型后,缓存到字典
    /// </summary>
    public interface ILazyServiceProvider
    {
        T LazyGetRequiredService<T>();

        object LazyGetRequiredService(Type serviceType);

        T LazyGetService<T>();

        object LazyGetService(Type serviceType);

        T LazyGetService<T>(T defaultValue);

        object LazyGetService(Type serviceType, object defaultValue);

        object LazyGetService(Type serviceType, Func<IServiceProvider, object> factory);

        T LazyGetService<T>(Func<IServiceProvider, object> factory);
    }
}
