using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Framework.AspNetCore.Mvc.Mvc
{
    public static class ActionResultHelper
    {
        public static List<Type> ObjectResultTypes { get; }

        static ActionResultHelper()
        {
            ObjectResultTypes = new List<Type>
            {
                typeof(JsonResult),
                typeof(ObjectResult),
                typeof(NoContentResult)
            };
        }

        /// <summary>
        /// 判断控制器Action的返回值是否是IActionResult、JsonResult、ObjectResult、NoContentResult
        /// </summary>
        /// <param name="returnType"></param>
        /// <param name="excludeTypes"></param>
        /// <returns></returns>
        public static bool IsObjectResult(Type returnType, params Type[] excludeTypes)
        {
            //获取控制器Action的返回值
            returnType = AsyncHelper.UnwrapTask(returnType);

            if (!excludeTypes.IsNullOrEmpty() && excludeTypes.Any(t => t.IsAssignableFrom(returnType)))
            {
                return false;
            }

            if (!typeof(IActionResult).IsAssignableFrom(returnType))
            {
                return true;
            }

            return ObjectResultTypes.Any(t => t.IsAssignableFrom(returnType));
        }
    }
}
