using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Ms.Configuration.Abstracts
{
    public interface IConfigurationProvider
    {

        /// <summary>
        /// 加载ConfigurationSource中的数据
        /// </summary>
        void Load();

        IChangeToken GetReloadToken();

        /// <summary>
        /// 根据key获取Data中的value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGet(string key, out string value);

        /// <summary>
        /// 添加Item到Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);

        IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath);
    }
}
