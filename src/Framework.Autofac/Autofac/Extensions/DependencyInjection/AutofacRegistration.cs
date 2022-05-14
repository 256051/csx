//using Autofac.Builder;
//using Autofac.Extensions.Builder;
//using Framework.Autofac;
//using Framework.Core;
//using Framework.Core.Configurations;
//using Framework.Core.Dependency;
//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Diagnostics.CodeAnalysis;
//using System.Reflection;

//namespace Autofac.Extensions.DependencyInjection
//{
//    public static class AutofacRegistration
//    {
//        /// <summary>
//        /// service转成build netcore di->autofac di
//        /// </summary>
//        /// <param name="builder"></param>
//        /// <param name="services"></param>
//        public static void Populate(
//          this ContainerBuilder builder,
//          IServiceCollection services)
//        {
//            Populate(builder, services, null);
//        }

//        public static void Populate(
//            this ContainerBuilder builder,
//            IServiceCollection services,
//            object lifetimeScopeTagForSingletons)
//        {
//            if (services == null)
//            {
//                throw new ArgumentNullException(nameof(services));
//            }

//            //设置AutofacServiceProvider永远不会被容器释放
//            builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>().ExternallyOwned();
//            var autofacServiceScopeFactory = typeof(AutofacServiceProvider).Assembly.GetType("Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory");
//            if (autofacServiceScopeFactory == null)
//            {
//                throw new FrameworkException("Unable get type of Autofac.Extensions.DependencyInjection.AutofacServiceScopeFactory!");
//            }

//            //注入autofacServiceScopeFactory exposetype(暴露类型)为IServiceScopeFactory
//            builder.RegisterType(autofacServiceScopeFactory).As<IServiceScopeFactory>();

//            Register(builder, services, lifetimeScopeTagForSingletons);
//        }

//        [SuppressMessage("CA2000", "CA2000", Justification = "Registrations created here are disposed when the built container is disposed.")]
//        private static void Register(
//            ContainerBuilder builder,
//            IServiceCollection services,
//            object lifetimeScopeTagForSingletons)
//        {
//            var registrationActionList = services.GetRegistrationActionList();

//            foreach (var descriptor in services)
//            {
//                //普通转换规则
//                if (descriptor.ImplementationType != null)
//                {
//                    var serviceTypeInfo = descriptor.ServiceType.GetTypeInfo();
//                    //泛型
//                    if (serviceTypeInfo.IsGenericTypeDefinition)
//                    {
//                        builder
//                            .RegisterGeneric(descriptor.ImplementationType)
//                            .As(descriptor.ServiceType)
//                            .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
//                            .FindConstructorsWith(new AutofacConstructorFinder())
//                            .ConfigureConventions(registrationActionList);
//                    }
//                    else
//                    {
//                        builder
//                            .RegisterType(descriptor.ImplementationType)
//                            .As(descriptor.ServiceType)
//                            .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
//                            .FindConstructorsWith(new AutofacConstructorFinder())
//                            .ConfigureConventions(registrationActionList);
//                    }
//                }
//                //如果注入类型时带工厂的(这些可能根据业务动态释出指定的实现类型)
//                else if (descriptor.ImplementationFactory != null)
//                {
//                    var registration = RegistrationBuilder.ForDelegate(descriptor.ServiceType, (context, parameters) =>
//                    {
//                        var serviceProvider = context.Resolve<IServiceProvider>();
//                        return descriptor.ImplementationFactory(serviceProvider);
//                    })
//                    .ConfigureLifecycle(descriptor.Lifetime, lifetimeScopeTagForSingletons)
//                    .CreateRegistration();
//                    builder.RegisterComponent(registration);
//                }
//                else
//                {
//                    builder
//                        .RegisterInstance(descriptor.ImplementationInstance)
//                        .As(descriptor.ServiceType)
//                        .ConfigureLifecycle(descriptor.Lifetime, null);
//                }
//            }
//        }


//        private static IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> ConfigureLifecycle<TActivatorData, TRegistrationStyle>(
//            this IRegistrationBuilder<object, TActivatorData, TRegistrationStyle> registrationBuilder,
//            ServiceLifetime lifecycleKind,
//            object lifetimeScopeTagForSingleton)
//        {
//            switch (lifecycleKind)
//            {
//                case ServiceLifetime.Singleton:
//                    if (lifetimeScopeTagForSingleton == null)
//                    {
//                        registrationBuilder.SingleInstance();
//                    }
//                    else
//                    {
//                        registrationBuilder.InstancePerMatchingLifetimeScope(lifetimeScopeTagForSingleton);
//                    }

//                    break;
//                case ServiceLifetime.Scoped:
//                    registrationBuilder.InstancePerLifetimeScope();
//                    break;
//                case ServiceLifetime.Transient:
//                    registrationBuilder.InstancePerDependency();
//                    break;
//            }

//            return registrationBuilder;
//        }
//    }
//}
