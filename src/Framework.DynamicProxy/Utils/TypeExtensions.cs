using System;

namespace Framework.DynamicProxy
{
    internal static class TypeExtensions
	{
		public static string GetBestName(this Type type)
		{
			return type.FullName ?? type.Name;
		}
	}
}
