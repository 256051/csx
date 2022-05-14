using Framework.Core.Dependency;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Framework.AspNetCore.Swagger
{
    public class XmlElementFinder : IXmlElementFinder, ISingleton
    {
        private List<XElement> _elements;
        public XmlElementFinder()
        {
            _elements = new List<XElement>();
            var files = XmlFilesProvider.GetFiles();
            foreach (var file in files)
            {
                var xDocument = XDocument.Load(file);
                _elements.AddIfNotContains(
                xDocument.Descendants("member")
               );
            }
        }

        public string GetEnumSummary(string fildName)
        {
            string result=default(string);
            for(var i=0;i< _elements.Count;i++)
            {
                var element = _elements[i];
                if(element.Attributes("name").Select(s=>s.Value).Contains($"F:{fildName}"))
                {
                    result = element.Descendants("summary").FirstOrDefault().Value;
                }
            }
            if (string.IsNullOrEmpty(result))
                return "获取枚举值失败";
            return result;
        }

        public bool IsTypeExisted(string typeName)
        {
            return _elements.Attributes("name")
               .Select(s => s.Value).Contains($"T:{typeName}");
        }
    }
}
