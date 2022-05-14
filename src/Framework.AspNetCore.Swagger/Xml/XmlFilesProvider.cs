using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Framework.AspNetCore.Swagger
{
    /// <summary>
    /// xml文件provider 提供应用程序中所有的xml文件
    /// </summary>
    public class XmlFilesProvider
    {
        private static List<string> _xmlFiles;
        static XmlFilesProvider()
        {
            _xmlFiles = new List<string>();
        }
        public static List<string> GetFiles()
        {
            if (_xmlFiles.Count == 0)
            {
                var runDirectory = Path.GetDirectoryName(typeof(ApplicationConfigurationExtension).Assembly.Location);
                var xmlFiles = Directory.GetFileSystemEntries(runDirectory)
                    .Where(file => Regex.IsMatch(Path.GetFileName(file), ".xml$", RegexOptions.IgnoreCase | RegexOptions.Multiline))
                    .ToList();
                _xmlFiles.AddIfNotContains(xmlFiles);
            }
            return _xmlFiles;
        }
    }
}
