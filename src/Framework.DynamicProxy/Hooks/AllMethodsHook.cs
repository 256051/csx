using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy
{
	/// <summary>
	/// 拦截全部方法的钩子
	/// </summary>
    public class AllMethodsHook : IProxyGenerationHook
    {
		/// <summary>
		/// 钩子跳过的类型
		/// </summary>
		protected static readonly ICollection<Type> SkippedTypes = new[]{
			typeof(object),
			typeof(MarshalByRefObject),
			typeof(ContextBoundObject)
		};
	}
}
