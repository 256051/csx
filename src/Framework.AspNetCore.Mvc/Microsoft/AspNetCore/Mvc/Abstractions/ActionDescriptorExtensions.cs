using Framework.AspNetCore.Mvc.Mvc;
using Framework.Core;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Reflection;

namespace Microsoft.AspNetCore.Mvc.Abstractions
{
    public static class ActionDescriptorExtensions
    {
        /// <summary>
        /// 判断当前上下文的请求是不是控制器请求
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        public static bool IsControllerAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor is ControllerActionDescriptor;
        }

        /// <summary>
        /// 将当前请求上下文强制转换成控制器请求  前提当前请求必须是访问控制器的请求
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        public static ControllerActionDescriptor AsControllerActionDescriptor(this ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.IsControllerAction())
            {
                throw new FrameworkException($"{nameof(actionDescriptor)} should be type of {typeof(ControllerActionDescriptor).AssemblyQualifiedName}");
            }

            return actionDescriptor as ControllerActionDescriptor;
        }

        /// <summary>
        /// 获取当前请求上下文访问的控制器方法
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        public static MethodInfo GetMethodInfo(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.AsControllerActionDescriptor().MethodInfo;
        }

        /// <summary>
        /// 获取控制其方法的返回值
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <returns></returns>
        public static bool HasObjectResult(this ActionDescriptor actionDescriptor)
        {
            return ActionResultHelper.IsObjectResult(actionDescriptor.GetMethodInfo().ReturnType);
        }
    }
}
