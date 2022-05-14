using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.DynamicProxy
{
    public interface IProxyGenerator
    {
        /// <summary>
        /// 生成类代理
        /// </summary>
        /// <param name="classNeedProxy">需要生成代理类的类型</param>
        /// <param name="interceptors">拦截器集合</param>
        /// <returns></returns>
        object CreateClassProxy(Type classNeedProxy, params IInterceptor[] interceptors);
    }
}
