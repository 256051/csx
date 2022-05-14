using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Framework.AspNetCore.Swagger
{
    /// <summary>
    /// 认证授权过滤器
    /// </summary>
    public class AuthorizationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter() {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Required = false,
                Description="登录返回的访问令牌"
            });
        }
    }
}
