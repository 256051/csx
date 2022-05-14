using Framework.Core.Dependency;
using Microsoft.Extensions.Options;
using System;

namespace Framework.Json.SystemTextJson
{
    public class SystemTextJsonUnsupportedTypeMatcher : ITransient
    {
        protected SystemTextJsonSerializerOptions Options { get; }

        public SystemTextJsonUnsupportedTypeMatcher(IOptions<SystemTextJsonSerializerOptions> options)
        {
            Options = options.Value;
        }

        public virtual bool Match(Type type)
        {
            return Options.UnsupportedTypes.Contains(type);
        }
    }
}
