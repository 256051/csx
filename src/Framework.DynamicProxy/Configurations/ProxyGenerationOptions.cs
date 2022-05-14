using System;
using System.Collections.Generic;

namespace Framework.DynamicProxy
{
    /// <summary>
    /// 代理生成设置
    /// </summary>
    public class ProxyGenerationOptions
    {
        /// <summary>
        /// 默认代理生成设置
        /// </summary>
        public static readonly ProxyGenerationOptions Default = new ProxyGenerationOptions();

        /// <summary>
        /// 接口代理的基本类型
        /// </summary>
        public Type BaseTypeForInterfaceProxy { get; set; }

        /// <summary>
        /// 钩子类型
        /// </summary>
        public IProxyGenerationHook Hook { get; set; }

        private List<object> mixins;

        public ProxyGenerationOptions() : this(new AllMethodsHook())
        {

        }

        public ProxyGenerationOptions(IProxyGenerationHook hook)
        {
            BaseTypeForInterfaceProxy = typeof(object);
            Hook = hook;
        }

        private MixinData mixinData;
        public void Initialize()
        {
            if (mixinData == null)
            {
                try
                {
                    mixinData = new MixinData(mixins);
                }
                catch (ArgumentException ex)
                {
                    throw new InvalidOperationException(
                        "There is a problem with the mixins added to this ProxyGenerationOptions. See the inner exception for details.", ex);
                }
            }
        }
    }
}
