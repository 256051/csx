using System;
using System.Threading;
using Xunit;

namespace Framework.RestSharp.Tests
{
    public class GetTest : RestSharpTest
    {
        [Fact]
        public void Test1()
        {
            try
            {
                var result = AsyncHelper.RunSync(() =>
                      {
                          return _httpRestClient.GetAsync("/api/Account/test?Data=get«Î«Û≤‚ ‘", "http://localhost:5000");
                      });
            }
            catch 
            {

                throw;
            }
            //var a = _jsonSerializer.Deserialize<ADto>(result);
        }
    }

}
