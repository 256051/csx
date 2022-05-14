using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core.Dependency
{
    /// <summary>
    /// 默认类型注册器
    /// </summary>
    public class DefaultConventionalRegistrar : ConventionalRegistrarBase
    {

        private List<Type> _lifeTimes = new List<Type>() {typeof(ISingleton), typeof(ITransient), typeof(IScoped), typeof(IReplace) };

        //todo可以封装下
        public override void AddType(IServiceCollection collection, Type type)
        {
            if (typeof(ISingleton).IsAssignableFrom(type))
            {
                var interfaces = type.GetInterfaces().Where(i => i != typeof(ISingleton)).ToArray();
                foreach (var inter in interfaces)
                {
                    collection.Add(ServiceDescriptor.Describe(
                        inter,
                        type,
                        ServiceLifetime.Singleton
                    ));
                }
                collection.Add(ServiceDescriptor.Describe(
                     type,
                     type,
                     ServiceLifetime.Singleton
                 ));
            }
            if (typeof(ITransient).IsAssignableFrom(type))
            {
                var interfaces = type.GetInterfaces().Where(i => i != typeof(ITransient)).ToArray();
                foreach (var inter in interfaces)
                {
                    collection.Add(ServiceDescriptor.Describe(
                        inter,
                        type,
                        ServiceLifetime.Transient
                    ));
                }
                collection.Add(ServiceDescriptor.Describe(
                       type,
                       type,
                       ServiceLifetime.Transient
                  ));
            }
            if (typeof(IScoped).IsAssignableFrom(type))
            {
                var interfaces = type.GetInterfaces().Where(i => i != typeof(IScoped)).ToArray();
                foreach (var inter in interfaces)
                {
                    collection.Add(ServiceDescriptor.Describe(
                        inter,
                        type,
                        ServiceLifetime.Scoped
                    ));
                }
                collection.Add(ServiceDescriptor.Describe(
                        type,
                        type,
                        ServiceLifetime.Scoped
                    ));
            }
            if (typeof(IReplace).IsAssignableFrom(type))
            {
                var serviceType = type.GetInterfaces().FirstOrDefault(w =>! _lifeTimes.Contains(w));
                var originType = collection.FirstOrDefault(f => f.ServiceType == serviceType).ImplementationType;
                var originTypeInterface = originType.GetInterfaces().Where(w => !_lifeTimes.Contains(w)).FirstOrDefault();
                var lifeTime = collection.FirstOrDefault(f => f.ServiceType== originTypeInterface).Lifetime;
                collection.Replace(ServiceDescriptor.Describe(
                         originTypeInterface,
                         type,
                         lifeTime
                     ));
            }
        }

        /// <summary>
        /// 获取类型的生命周期
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected virtual Type GetTypeLifeTime(Type type)
        {
            var lifeTime = default(Type);
            _lifeTimes.ForEach(lt =>
            {
                if (lt.IsAssignableFrom(type))
                {
                    lifeTime = lt;
                    return;
                }
            });
            return lifeTime == default(Type)?null:lifeTime;
        }
    }
}
