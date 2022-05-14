using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Framework.DynamicProxy
{
    public static class ProxyUtil
    {
		private static readonly SynchronizedDictionary<Assembly, bool> internalsVisibleToDynamicProxy = new SynchronizedDictionary<Assembly, bool>();

		internal static bool IsAccessibleType(Type target)
		{
			var isPublic = target.IsPublic || target.IsNestedPublic;
			if (isPublic)
			{
				return true;
			}

			//判断类型是否为嵌套类型
			var isTargetNested = target.IsNested;
			/*
			 * 满足一下条件isNestedAndInternal为true
			 * 1、当前类型为嵌套类型
			 * 2、嵌套类型在自己所在的程序集中可见 Internal
			 * 3、嵌套类型在自己的家族类中可见或者在自己程序集中可见
			 * */
			var isNestedAndInternal = isTargetNested && (target.IsNestedAssembly || target.IsNestedFamORAssem);
			//Internal但是不属于嵌套类型
			var isInternalNotNested = target.IsVisible == false && isTargetNested == false;

			//Internal但是不属于嵌套类型 嵌套类型且Internal
			var isInternal = isInternalNotNested || isNestedAndInternal;
			if (isInternal && AreInternalsVisibleToDynamicProxy(target.Assembly))
			{
				return true;
			}

			return false;
		}

		internal static bool AreInternalsVisibleToDynamicProxy(Assembly asm)
		{
			return internalsVisibleToDynamicProxy.GetOrAdd(asm, a =>
			{
				var internalsVisibleTo = asm.GetCustomAttributes<InternalsVisibleToAttribute>();
				return internalsVisibleTo.Any(attr => attr.AssemblyName.Contains(ModuleScope.DEFAULT_ASSEMBLY_NAME));
			});
		}
	}
}
