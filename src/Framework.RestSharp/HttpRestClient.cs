using Framework.Core.Dependency;
using Framework.Json;
using RestSharp;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Framework.RestSharp
{
    public class HttpRestClient : IHttpRestClient,ISingleton
    {
        private IJsonSerializer _jsonSerializer;
        public HttpRestClient(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public async Task<string> PostAsync<TRequest>(string url, TRequest request, string adress = "") where TRequest : class
        {
            var client = new RestClient(adress);
            client.ThrowOnDeserializationError = true;
            client.FailOnDeserializationError = true;
            client.ThrowOnAnyError = true;
            client.Encoding = Encoding.UTF8;
            var restRequest = new RestRequest(url);
            restRequest.AddJsonBody(request);
            restRequest.Method = Method.POST;
            var post = await client.ExecuteAsync(restRequest);
            if (post.ErrorException != null || post.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRestClientRequestException(null, GetExceptionMessage(url, adress, post.ErrorException??null, (int)post.StatusCode, _jsonSerializer.Serialize(request)), null, post.ErrorException);
            }
            return post.Content;
        }

        public async Task<string> GetAsync(string url, string adress = "") 
        {
            var client = new RestClient(adress);
            client.ThrowOnDeserializationError = true;
            client.FailOnDeserializationError = true;
            client.ThrowOnAnyError = true;
            client.Encoding = Encoding.UTF8;
            var restRequest = new RestRequest(url);
            restRequest.Method = Method.GET;
            var post = await client.ExecuteAsync(restRequest);
            if (post.ErrorException != null || post.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new HttpRestClientRequestException(null, GetExceptionMessage(url, adress, post.ErrorException ?? null, (int)post.StatusCode), null, post.ErrorException);
            }
            return post.Content;
        }

        public string GetExceptionMessage(string url,string adress,Exception exception,int statusCode,string request="")
        {
            var builder = new StringBuilder();
            builder.Append($"访问地址'{url}{adress}失败',响应状态码:'{statusCode}'");
            builder.AppendLine();
            builder.Append($"请求参数如下:{request}");
            if (exception != null)
            {
                builder.AppendLine();
                builder.Append($"异常信息如下:{exception.Message},堆栈信息如下:{exception.StackTrace}");
            }
            builder.AppendLine();
            return builder.ToString();
        }
    }
}
