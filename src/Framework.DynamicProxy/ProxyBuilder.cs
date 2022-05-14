using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy
{
    public class ProxyBuilder : IProxyBuilder
    {
        public ILogger Logger { get; set; }

        private readonly ModuleScope scope;
        public ModuleScope ModuleScope
        {
            get { return scope; }
        }

        public ProxyBuilder() : this(new ModuleScope())
        {

        }

        public ProxyBuilder(ModuleScope scope)
        {
            this.scope = scope;
        }

        public Type CreateClassProxyType(Type classToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options)
        {
            ProxyBuilderHelper.AssertValidType(classToProxy, nameof(classToProxy));
            ProxyBuilderHelper.AssertValidTypes(additionalInterfacesToProxy, nameof(additionalInterfacesToProxy));
           
            var generator = new ClassProxyGenerator(scope, classToProxy, additionalInterfacesToProxy, options) { Logger = logger };
            return generator.GetProxyType();
        }
    }
}
