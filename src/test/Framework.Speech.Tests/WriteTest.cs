using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Framework.Speech.Tests
{
    public class WriteTest: SpeechTest
    {
        [Fact]
        public async Task Test1()
        {
            await TextToSpeecher.ConvertAsync("if 袁建成=傻逼 return 6 6 6", $"32423.mp3", CancellationToken.None);
        }

        [Fact]
        public void SpeakTest()
        {
             TextToSpeecher.Speak("饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批饥渴的一批", CancellationToken.None);
        }
    }
}
