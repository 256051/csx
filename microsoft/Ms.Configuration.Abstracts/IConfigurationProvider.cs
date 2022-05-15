using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Ms.Configuration.Abstracts
{
    public interface IConfigurationProvider
    {

        /// <summary>
        /// ����ConfigurationSource�е�����
        /// </summary>
        void Load();

        IChangeToken GetReloadToken();

        /// <summary>
        /// ����key��ȡData�е�value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGet(string key, out string value);

        /// <summary>
        /// ���Item��Data
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);

        IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath);
    }
}
