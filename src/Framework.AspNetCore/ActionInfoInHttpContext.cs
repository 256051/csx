using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Framework.Web.AspNetCore
{
    /// <summary>
    /// 当前控制器Action上下文  如果中间件拦截的是控制器
    /// </summary>
    public class ActionInfoInHttpContext
    {
        /// <summary>
        /// 判断是否是标准的控制器Action返回值
        /// </summary>
        public bool IsObjectResult { get; set; }
        
        /// <summary>
        /// 控制器名称
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// 控制器Action名称
        /// </summary>
        public string ActionName { get; set; }
    }
}
