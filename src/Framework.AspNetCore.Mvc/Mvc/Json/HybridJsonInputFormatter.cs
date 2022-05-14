using Framework.Json.SystemTextJson;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.Threading.Tasks;

namespace Framework.AspNetCore.Mvc.Mvc.Json
{
    /// <summary>
    /// 混合Json输入格式化类
    /// </summary>
    public class HybridJsonInputFormatter : TextInputFormatter, IInputFormatterExceptionPolicy
    {
        private readonly SystemTextJsonInputFormatter _systemTextJsonInputFormatter;
        private readonly NewtonsoftJsonInputFormatter _newtonsoftJsonInputFormatter;

        public HybridJsonInputFormatter(SystemTextJsonInputFormatter systemTextJsonInputFormatter, NewtonsoftJsonInputFormatter newtonsoftJsonInputFormatter)
        {
            _systemTextJsonInputFormatter = systemTextJsonInputFormatter;
            _newtonsoftJsonInputFormatter = newtonsoftJsonInputFormatter;

            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);

            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.TextJson);
            SupportedMediaTypes.Add(MediaTypeHeaderValues.ApplicationAnyJsonSyntax);
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding)
        {
            return await GetTextInputFormatter(context).ReadRequestBodyAsync(context, encoding);
        }

        /// <summary>
        /// 判断传入的类型该使用哪种格式化器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual TextInputFormatter GetTextInputFormatter(InputFormatterContext context)
        {
            var typesMatcher = context.HttpContext.RequestServices.GetRequiredService<SystemTextJsonUnsupportedTypeMatcher>();

            if (!typesMatcher.Match(context.ModelType))
            {
                return _systemTextJsonInputFormatter;
            }

            return _newtonsoftJsonInputFormatter;
        }

        /// <summary>
        /// 将异常输出到model state中
        /// </summary>
        public virtual InputFormatterExceptionPolicy ExceptionPolicy => InputFormatterExceptionPolicy.AllExceptions;
    }
}
