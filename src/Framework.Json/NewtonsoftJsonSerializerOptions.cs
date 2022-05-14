using Framework.Core.Collections;
using Newtonsoft.Json;

namespace Framework.Json
{
    public class NewtonsoftJsonSerializerOptions
    {
        public ITypeList<JsonConverter> Converters { get; }

        public NewtonsoftJsonSerializerOptions()
        {
            Converters = new TypeList<JsonConverter>();
        }
    }
}
