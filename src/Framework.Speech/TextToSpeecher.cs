using Framework.Core.Dependency;
using NAudio.Lame;
using NAudio.Wave;
using System.IO;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Speech
{
    public class TextToSpeecher : ITextToSpeecher,ISingleton
    {
        public async Task ConvertAsync(string text, string savePath, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (SpeechSynthesizer  speaker = new SpeechSynthesizer())
            {
                speaker.Volume = 100;
                using (var ms = new MemoryStream())
                {
                    speaker.SetOutputToWaveStream(ms);
                    speaker.Speak(text);
                    ConvertWavStreamToMp3File(ms, savePath);
                }
            }
            await Task.CompletedTask;
        }

        public static void ConvertWavStreamToMp3File(MemoryStream ms, string savePath)
        {
            ms.Seek(0, SeekOrigin.Begin);
            using (var reader = new WaveFileReader(ms))
            using (var writer = new LameMP3FileWriter(savePath, reader.WaveFormat, LAMEPreset.VBR_90))
            {
                reader.CopyTo(writer);
            }
        }

        public void Speak(string text, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (SpeechSynthesizer speaker = new SpeechSynthesizer())
            {
                speaker.Speak(text);
            }
        }

        public void Pause(SpeechSynthesizer speaker,CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            speaker.Pause();
        }
    }
}
