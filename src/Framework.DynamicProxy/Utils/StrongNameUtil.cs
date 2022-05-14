using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Framework.DynamicProxy
{
	internal static class StrongNameUtil
    {
		private static readonly IDictionary<Assembly, bool> signedAssemblyCache = new Dictionary<Assembly, bool>();
		private static readonly object lockObject = new object();
		public static bool IsAssemblySigned(this Assembly assembly)
		{
			lock (lockObject)
			{
				if (signedAssemblyCache.TryGetValue(assembly, out var isSigned) == false)
				{
					isSigned = assembly.ContainsPublicKey();
					signedAssemblyCache.Add(assembly, isSigned);
				}
				return isSigned;
			}
		}

		private static bool ContainsPublicKey(this Assembly assembly)
		{
			// Pulled from a comment on http://www.flawlesscode.com/post/2008/08/Mocking-and-IOC-in-Silverlight-2-Castle-Project-and-Moq-ports.aspx
			return assembly.FullName != null && !assembly.FullName.Contains("PublicKeyToken=null");
		}
	}
}
