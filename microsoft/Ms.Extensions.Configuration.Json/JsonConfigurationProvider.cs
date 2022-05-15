using Ms.Configuration.FileExtensions;
using System;
using System.IO;
using System.Text.Json;

namespace Ms.Extensions.Configuration.Json
{
    public class JsonConfigurationProvider : FileConfigurationProvider
    {
        public JsonConfigurationProvider(JsonConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            try
            {
                Data = JsonConfigurationFileParser.Parse(stream);
            }
            catch (JsonException e)
            {
                throw new FormatException(e.Message);
            }
        }
    }
}
