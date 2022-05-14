using Framework.Core.Dependency;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Framework.AspNetCore.Swagger.Filters
{
    public class EnumSchemaFilter : ISchemaFilter,ISingleton
    {
        private IXmlElementFinder _xmlElementFinder;
        public EnumSchemaFilter(IXmlElementFinder xmlElementFinder)
        {
            _xmlElementFinder = xmlElementFinder;
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema.Enum != null && schema.Enum.Count > 0 &&
                context.Type != null && context.Type.IsEnum)
            {
                if(!_xmlElementFinder.IsTypeExisted(context.Type.FullName)) return;
                schema.Description += "<ul>";
                var fields=context.Type.GetFields().Where(field=> field.Name!= "value__").ToList();
                foreach (var field in fields)
                {
                    var enumValue = (int)field.GetValue(null);
                    var summary = _xmlElementFinder.GetEnumSummary($"{context.Type.FullName}.{field.Name}");
                    schema.Description += $"<li><i>{enumValue}</i> - {summary.Trim()}</li> ";
                }
                schema.Description += "</ul>";
                schema.Enum = null;
            }
        }
    }
}
