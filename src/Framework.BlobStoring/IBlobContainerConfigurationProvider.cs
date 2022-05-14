using System;
using System.Collections.Generic;
using System.Text;

namespace Framework.BlobStoring
{
    public interface IBlobContainerConfigurationProvider
    {
        /// <summary>
        /// 根据容器名称,获取容器配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        BlobContainerConfiguration Get(string name);
    }
}
