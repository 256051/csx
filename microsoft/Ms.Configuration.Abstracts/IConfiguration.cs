using Microsoft.Extensions.Primitives;
using System.Collections.Generic;

namespace Ms.Configuration.Abstracts
{
    public interface IConfiguration
    {
        /// <summary>
        /// �ֵ����ʽ���  root[key]
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string this[string key] { get; set; }

        IEnumerable<IConfigurationSection> GetChildren();

        IConfigurationSection GetSection(string key);


         IChangeToken GetReloadToken();
    }
}
