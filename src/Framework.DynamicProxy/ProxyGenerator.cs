using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy
{
    public class ProxyGenerator : IProxyGenerator
    {
		private ILogger logger = NullLogger.Instance;

		private readonly IProxyBuilder proxyBuilder;

		public ILogger Logger
		{
			get { return logger; }
			set
			{
				logger = value;
				proxyBuilder.Logger = value;
			}
		}

		public IProxyBuilder ProxyBuilder
		{
			get { return proxyBuilder; }
		}

		public ProxyGenerator(IProxyBuilder builder)
		{
			proxyBuilder = builder;

			Logger = new TraceLogger("Castle.DynamicProxy", LoggerLevel.Warn);
		}

		public object CreateClassProxy(Type classNeedProxy, params IInterceptor[] interceptors)
        {
			return CreateClassProxy(classNeedProxy, null, ProxyGenerationOptions.Default,
									null, interceptors);
		}

		public virtual object CreateClassProxy(Type classNeedProxy, Type[] additionalInterfacesToProxy,
									   ProxyGenerationOptions options,
									   object[] constructorArguments, params IInterceptor[] interceptors)
		{
			if (classNeedProxy == null)
			{
				throw new ArgumentNullException(nameof(classNeedProxy));
			}
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}
			if (!classNeedProxy.IsClass)
			{
				throw new ArgumentException("'classNeedProxy' must be a class", nameof(classNeedProxy));
			}

			TypeHelper.CheckNotGenericTypeDefinition(classNeedProxy, nameof(classNeedProxy));
			TypeHelper.CheckNotGenericTypeDefinitions(additionalInterfacesToProxy, nameof(additionalInterfacesToProxy));

			var proxyType = CreateClassProxyType(classToProxy, additionalInterfacesToProxy, options);

			// create constructor arguments (initialized with mixin implementations, interceptors and target type constructor arguments)
			var arguments = BuildArgumentListForClassProxy(options, interceptors);
			if (constructorArguments != null && constructorArguments.Length != 0)
			{
				arguments.AddRange(constructorArguments);
			}
			return CreateClassProxyInstance(proxyType, arguments, classToProxy, constructorArguments);
		}

		protected Type CreateClassProxyType(Type classToProxy, Type[] additionalInterfacesToProxy,ProxyGenerationOptions options)
		{
			return ProxyBuilder.CreateClassProxyType(classToProxy, additionalInterfacesToProxy, options);
		}
	}
}
