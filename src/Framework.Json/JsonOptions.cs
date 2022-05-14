using Framework.Core.Collections;
using Framework.Timing;

namespace Framework.Json
{
    public class JsonOptions
    {
        /// <summary>
        /// 默认时间序列化格式
        /// </summary>
        public string DefaultDateTimeFormat { get; set; } 

        /// <summary>
        /// 混合序列化 在api使用system.text.json的基础上集成newton(功能相对较多)
        /// </summary>
        public bool UseHybridSerializer { get; set; } = true;

        /// <summary>
        /// Json序列化 Provider集合暂时只有newton和text.json
        /// </summary>
        public ITypeList<IJsonSerializerProvider> Providers { get; }

        public JsonOptions()
        {
            Providers = new TypeList<IJsonSerializerProvider>();
            DefaultDateTimeFormat = DateTimeFormats.LongTime;
        }
    }
}
