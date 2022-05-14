using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy
{
    public static class TypeHelper
    {
		/// <summary>
		/// 确保当前类型不是泛型
		/// </summary>
		/// <param name="type"></param>
		/// <param name="argumentName"></param>
		public static void CheckNotGenericTypeDefinition(Type type, string argumentName)
		{
			if (type != null && type.IsGenericTypeDefinition)
			{
				throw new ArgumentException(
					$"Can not create proxy for type {type.GetBestName()} because it is an open generic type.",
					argumentName);
			}
		}

		/// <summary>
		/// 确保当前类型集合中的类型全都不是泛型
		/// </summary>
		/// <param name="types"></param>
		/// <param name="argumentName"></param>
		public static void CheckNotGenericTypeDefinitions(IEnumerable<Type> types, string argumentName)
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
