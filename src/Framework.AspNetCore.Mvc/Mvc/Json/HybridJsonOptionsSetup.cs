using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Framework.AspNetCore.Mvc.Mvc.Json
{
    public class HybridJsonOptionsSetup : IConfigureOptions<MvcOptions>
    {
        private readonly IOptions<JsonOptions> _jsonOptions;
        private readonly IOptions<MvcNewtonsoftJsonOptions> _mvcNewtonsoftJsonOptions;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ArrayPool<char> _charPool;
        private readonly ObjectPoolProvider _objectPoolProvider;

        public HybridJsonOptionsSetup(
            IOptions<JsonOptions> jsonOptions,
            IOptions<MvcNewtonsoftJsonOptions> mvcNewtonsoftJsonOptions,
            ILoggerFactory loggerFactory,
            ArrayPool<char> charPool,
            ObjectPoolProvider objectPoolProvider)
        {
            _jsonOptions = jsonOptions;
            _mvcNewtonsoftJsonOptions = mvcNewtonsoftJsonOptions;
            _loggerFactory = loggerFactory;
            _charPool = charPool;
            _objectPoolProvider = objectPoolProvider;
        }



        public void Configure(MvcOptions options)
        {
            //输入
            var systemTextJsonInputFormatter = new SystemTextJsonInputFormatter(
               _jsonOptions.Value,
               _loggerFactory.CreateLogger<SystemTextJsonInputFormatter>());

            var newtonsoftJsonInputFormatter = new NewtonsoftJsonInputFormatter(
                _loggerFactory.CreateLogger<NewtonsoftJsonInputFormatter>(),
                _mvcNewtonsoftJsonOptions.Value.SerializerSettings,
                _charPool,
                _objectPoolProvider,
                options,
                _mvcNewtonsoftJsonOptions.Value);

            options.InputFormatters.RemoveType<SystemTextJsonInputFormatter>();
            options.InputFormatters.RemoveType<NewtonsoftJsonInputFormatter>();
            options.InputFormatters.Add(new HybridJsonInputFormatter(systemTextJsonInputFormatter, newtonsoftJsonInputFormatter));

            //输出
            var jsonSerializerOptions = _jsonOptions.Value.JsonSerializerOptions;
            if (jsonSerializerOptions.Encoder is null)
            {
                //SystemTextJson没有显示指定编码的话，使用不太严格的编码器，该编码器不会对所有非ASCII字符进行编码。
                //没有这行代码,中文返回会有问题
                jsonSerializerOptions = new JsonSerializerOptions(jsonSerializerOptions)
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                };
            }

            var systemTextJsonOutputFormatter = new SystemTextJsonOutputFormatter(jsonSerializerOptions);
            var newtonsoftJsonOutputFormatter = new NewtonsoftJsonOutputFormatter(
                _mvcNewtonsoftJsonOptions.Value.SerializerSettings,
                _charPool,
                options);

            options.OutputFormatters.RemoveType<SystemTextJsonOutputFormatter>();
            options.OutputFormatters.RemoveType<NewtonsoftJsonOutputFormatter>();
            options.OutputFormatters.Add(new HybridJsonOutputFormatter(systemTextJsonOutputFormatter, newtonsoftJsonOutputFormatter));
        }
    }
}
