using System.Collections.Generic;

namespace Framework.AspNetCore.Swagger
{
    public class SwaggerOptions
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 版本描述
        /// </summary>
        public string VersionDescription { get; set; }

        /// <summary>
        /// 托管的模块
        /// </summary>
        public List<string> Modules { get; set; }
    }
}
