using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;

namespace Framework.Speech
{
    /// <summary>
    /// 文字转语音
    /// </summary>
    public interface ITextToSpeecher
    {
        /// <summary>
        /// 文字转语音
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="savePath">/*保存路径*/</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task ConvertAsync(string text,string savePath, CancellationToken cancellationToken);

        /// <summary>
        /// 播放文字内容同步
        /// </summary>
        /// <param name="text">内容</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        void Speak(string text, CancellationToken cancellationToken);

        /// <summary>
        /// 暂停播放
        /// </summary>
        /// <param name="cancellationToken"></param>
        void Pause(SpeechSynthesizer speaker,CancellationToken cancellationToken);
    }
}
