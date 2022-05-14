using Framework.Json;
using System;
using System.Threading;
using Xunit;

namespace Framework.RestSharp.Tests
{
    public class PostTest: RestSharpTest
    {
        [Fact]
        public void Test1()
        {
            try
            {
                var result = AsyncHelper.RunSync(() =>
                      {
                          return _httpRestClient.PostAsync("/api/Account/login", new LoginDto() { UserName = "基地管理员", Password = "123" }, "http://localhost:5000");
                      });
            }
            catch
            {
                throw;
            }
            //var a = _jsonSerializer.Deserialize<ADto>(result);
        }
    }

    public class LoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class ADto
    {
        public bool success { get; set; }

        public string message { get; set; }
    }

}
