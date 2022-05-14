using Framework.Core.Collections;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Framework.Json.SystemTextJson
{
    /// <summary>
    /// SystemTextJson序列化配置
    /// </summary>
    public class SystemTextJsonSerializerOptions
    {
        public JsonSerializerOptions JsonSerializerOptions { get; }

        /// <summary>
        /// 指定无法序列化的类型,交给newton序列化
        /// </summary>
        public ITypeList UnsupportedTypes { get; }

        public SystemTextJsonSerializerOptions()
        {
            JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                Encoder= JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            UnsupportedTypes = new TypeList();
        }
    }
}
