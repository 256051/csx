using System;
using System.Collections.Generic;

namespace Framework.DynamicProxy.Generators
{
    internal abstract class BaseProxyGenerator
    {
		private readonly ModuleScope _scope;

		protected readonly Type _targetType;

		protected readonly Type[] _interfaces;

		private ProxyGenerationOptions _proxyGenerationOptions;

		protected BaseProxyGenerator(ModuleScope scope, Type targetType, Type[] interfaces, ProxyGenerationOptions proxyGenerationOptions)
		{
			CheckNotGenericTypeDefinition(targetType, nameof(targetType));
			CheckNotGenericTypeDefinitions(interfaces, nameof(interfaces));

			_scope = scope;
			_targetType = targetType;
			_interfaces = TypeUtil.GetAllInterfaces(interfaces);
			_proxyGenerationOptions = proxyGenerationOptions;
			_proxyGenerationOptions.Initialize();
		}

		protected void CheckNotGenericTypeDefinition(Type type, string argumentName)
		{
			if (type != null && type.IsGenericTypeDefinition)
			{
				throw new ArgumentException("Type cannot be a generic type definition. Type: " + type.FullName, argumentName);
			}
		}

		protected void CheckNotGenericTypeDefinitions(IEnumerable<Type> types, string argumentName)
		{
			if (types == null)
			{
				return;
			}
			foreach (var t in types)
			{
				CheckNotGenericTypeDefinition(t, argumentName);
			}
		}
	}
}
