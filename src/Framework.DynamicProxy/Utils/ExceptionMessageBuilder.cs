using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Framework.DynamicProxy
{
    public class ExceptionMessageBuilder
    {
		internal static string CreateInstructionsToMakeVisible(Assembly targetAssembly)
		{
			string strongNamedOrNotIndicator = " not"; // assume not strong-named
			string assemblyToBeVisibleTo = "\"DynamicProxyGenAssembly2\""; // appropriate for non-strong-named

			if (targetAssembly.IsAssemblySigned())
			{
				strongNamedOrNotIndicator = "";
				assemblyToBeVisibleTo = ReferencesCastleCore(targetAssembly)
					? "InternalsVisible.ToDynamicProxyGenAssembly2"
					: '"' + InternalsVisible.ToDynamicProxyGenAssembly2 + '"';
			}

			var instructionsFormat =
				"Make it public, or internal and mark your assembly with " +
				"[assembly: InternalsVisibleTo({0})] attribute, because assembly {1} " +
				"is{2} strong-named.";

			var instructions = string.Format(instructionsFormat,
				assemblyToBeVisibleTo,
				targetAssembly.GetName().Name,
				strongNamedOrNotIndicator);
			return instructions;

			bool ReferencesCastleCore(Assembly ia)
			{
				return ia.GetReferencedAssemblies()
					.Any(r => r.FullName == Assembly.GetExecutingAssembly().FullName);
			}
		}

		public static string CreateMessageForInaccessibleType(Type inaccessibleType, Type typeToProxy)
		{
			var targetAssembly = typeToProxy.Assembly;

			string inaccessibleTypeDescription = inaccessibleType == typeToProxy
				? "it"
				: "type " + inaccessibleType.GetBestName();

			var messageFormat = "Can not create proxy for type {0} because {1} is not accessible. ";

			var message = string.Format(messageFormat,
				typeToProxy.GetBestName(),
				inaccessibleTypeDescription);

			var instructions = CreateInstructionsToMakeVisible(targetAssembly);

			return message + instructions;
		}
	}
}
