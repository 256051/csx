using System;
using System.Collections.Generic;

namespace Ms.Configuration.Abstracts
{
    public interface IConfigurationBuilder
    {
        /// <summary>
        /// 配置Provider集合 如PhysicalFileProvider等
        /// </summary>
        IDictionary<string, object> Properties { get; }

        /// <summary>
        /// 配置源集合
        /// </summary>
        IList<IConfigurationSource> Sources { get; }

        /// <summary>
        /// 添加配置源
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        IConfigurationBuilder Add(IConfigurationSource source);

        /// <summary>
        /// 根据Provider和Source生成IConfigurationRoot
        /// </summary>
        /// <returns></returns>
        IConfigurationRoot Build();
    }
}
