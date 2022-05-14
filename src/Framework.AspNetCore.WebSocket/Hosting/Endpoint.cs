using Microsoft.AspNetCore.Http;
using System;

namespace Framework.AspNetCore.WebSocket.Hosting
{
    /// <summary>
    /// 终结点
    /// </summary>
    public class Endpoint
    {
        public Endpoint(string name, string path, Type handlerType)
        {
            Name = name;
            Path = path;
            Handler = handlerType;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 访问url
        /// </summary>
        public PathString Path { get; set; }

        /// <summary>
        /// 对应的处理器
        /// </summary>
        public Type Handler { get; set; }
    }
}
