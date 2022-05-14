using System.Threading.Tasks;

namespace Framework.RestSharp
{
    /// <summary>
    /// 本地Http客户端代理 用于后台级别发出http请求 restful风格
    /// </summary>
    public interface IHttpRestClient
    {
        Task<string> PostAsync<TRequest>(string url, TRequest request, string adress = "") where TRequest:class;

        Task<string> GetAsync(string url, string adress = "");
    }
}
