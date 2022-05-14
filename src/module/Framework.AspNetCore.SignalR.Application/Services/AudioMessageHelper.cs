using Framework.Core.Dependency;
using Framework.Speech;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Application.Services
{
    public class AudioMessageHelper:ISingleton
    {
        private ITextToSpeecher _textToSpeecher;

        public static string TargetDirectory = "AudioFiles";

        public static string SaveDirectory = Path.Combine(Environment.CurrentDirectory, TargetDirectory);

        public AudioMessageHelper(ITextToSpeecher textToSpeecher)
        {
            _textToSpeecher = textToSpeecher;
        }

        public async Task<string> CreateAsync(string message,CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
            var fileName = $"{Guid.NewGuid()}.mp3";
            var filePath = Path.Combine(SaveDirectory, fileName);
            await _textToSpeecher.ConvertAsync(message, filePath, cancellationToken);
            if (File.Exists(filePath))
                return $"{TargetDirectory}/{fileName}";
            else
                throw new Exception($"消息:{message}生成音频文件失败");
        }
    }
}
