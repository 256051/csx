using Polly;
using Shouldly;
using System;
using Xunit;

namespace Framework.BlobStoring.FileSystem.Tests
{
    public class PolyTest
    {
        [Fact]
        public void Test1()
        {
            var index = 0;
             Policy.Handle<Exception>()
               .WaitAndRetry(2, retryCount => TimeSpan.FromSeconds(retryCount))
               .Execute(() =>
               {
                   index++;
                   if(index!=2)
                    throw new Exception("≤‚ ‘polly÷ÿ ‘");
               });
            index.ShouldBe(2);
        }
    }
}
