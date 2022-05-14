using System;
using System.Collections.Generic;

namespace Framework.DynamicProxy
{
	public static class ProxyBuilderHelper
    {
		public static void AssertValidType(Type target, string paramName)
        {
            AssertValidTypeForTarget(target, target, paramName);
        }

		public static void AssertValidTypes(IEnumerable<Type> targetTypes, string paramName)
		{
			if (targetTypes != null)
			{
				foreach (var t in targetTypes)
				{
					AssertValidType(t, paramName);
				}
			}
		}

		public static void AssertValidTypeForTarget(Type type, Type target, string paramName)
		{
			if (type.IsGenericTypeDefinition)
			{
				throw new ArgumentException(
					$"Can not create proxy for type {target.GetBestName()} because type {type.GetBestName()} is an open generic type.",
					paramName);
			}
			if (ProxyUtil.IsAccessibleType(type) == false)
			{
				throw new ArgumentException(ExceptionMessageBuilder.CreateMessageForInaccessibleType(type, target), paramName);
			}

			foreach (var typeArgument in type.GetGenericArguments())
			{
				AssertValidTypeForTarget(typeArgument, target, paramName);
			}
		}
	}
}
