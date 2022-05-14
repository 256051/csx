using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy
{
    /// <summary>
    /// 代理生成器
    /// </summary>
    public interface IProxyBuilder
    {
        /// <summary>
        /// 日志
        /// </summary>
        ILogger Logger { get; set; }

        /// <summary>
        /// 创建代理类
        /// </summary>
        /// <param name="classToProxy"></param>
        /// <param name="additionalInterfacesToProxy"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        Type CreateClassProxyType(Type classToProxy, Type[] additionalInterfacesToProxy, ProxyGenerationOptions options);
    }
}
