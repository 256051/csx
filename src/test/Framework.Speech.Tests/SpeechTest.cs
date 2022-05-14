using Framework.Test;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Speech.Tests
{
    public class SpeechTest : TestBase
    {
        protected ITextToSpeecher TextToSpeecher { get; set; }

        public SpeechTest()
        {
            TextToSpeecher = ServiceProvider.GetRequiredService<ITextToSpeecher>(); ;
        }

        protected override void LoadModules()
        {
            ApplicationConfiguration.UseSpeech();
        }
    }
}
