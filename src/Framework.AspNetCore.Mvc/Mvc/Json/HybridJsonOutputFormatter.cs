using Framework.Json.SystemTextJson;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Mvc.Mvc.Json
{
    public class HybridJsonOutputFormatter : TextOutputFormatter
    {
        private readonly SystemTextJsonOutputFormatter _systemTextJsonOutputFormatter;
        private readonly NewtonsoftJsonOutputFormatter _newtonsoftJsonOutputFormatter;

        public HybridJsonOutputFormatter(SystemTextJsonOutputFormatter systemTextJsonOutputFormatter, NewtonsoftJsonOutputFormatter newtonsoftJsonOutputFormatter)
        {
            _systemTextJsonOutputFormatter = systemTextJsonOutputFormatter;
            _newtonsoftJsonOutputFormatter = newtonsoftJsonOutputFormatter;

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            await GetTextInputFormatter(context).WriteResponseBodyAsync(context, selectedEncoding);
        }

        protected virtual TextOutputFormatter GetTextInputFormatter(OutputFormatterWriteContext context)
        {
            var typesMatcher = context.HttpContext.RequestServices.GetRequiredService<SystemTextJsonUnsupportedTypeMatcher>();
            if (!typesMatcher.Match(context.ObjectType))
            {
                return _systemTextJsonOutputFormatter;
            }

            return _newtonsoftJsonOutputFormatter;
        }
    }
}
