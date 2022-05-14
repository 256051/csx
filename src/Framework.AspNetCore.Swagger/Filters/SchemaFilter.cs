using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Framework.AspNetCore.Swagger.Filters
{
    /// <summary>
    /// schema过滤器
    /// </summary>
    public class SchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            RemoveDddDomainEntityProperties(schema, context);
        }

        /// <summary>
        /// 移除Framework.DDD.Domain中的实体
        /// 兼容开发的一些不规范操作,将数据库实体直接当视图模型用
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="context"></param>
        private void RemoveDddDomainEntityProperties(OpenApiSchema schema, SchemaFilterContext context)
        {
             List<string> _excludedProperties = new List<string>() {
            "CreationTime", "CreatorId", "LastModificationTime","LastModifierId",
            "IsDeleted","DeleterId","DeletionTime","Id"};

            if (schema?.Properties == null || schema?.Properties.Count==0 || context.Type == null)
                return;

            foreach (var property in schema.Properties)
            {
                if (_excludedProperties.Select(s=>s.ToLower()).Contains(property.Key.ToLower()))
                    schema.Properties.Remove(property.Key);
            }
        }
    }
}
