using IdentityModel.Tokens;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityModel.Protocols
{
    /// <summary>
    /// Retrieves metadata information using HttpClient.
    /// </summary>
    public class HttpDocumentRetriever : IDocumentRetriever
    {
        private HttpClient _httpClient;
        private static readonly HttpClient _defaultHttpClient = new HttpClient();

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDocumentRetriever"/> class.
        /// </summary>
        public HttpDocumentRetriever()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpDocumentRetriever"/> class with a specified httpClient.
        /// </summary>
        /// <param name="httpClient"><see cref="HttpClient"/></param>
        /// <exception cref="ArgumentNullException">'httpClient' is null.</exception>
        public HttpDocumentRetriever(HttpClient httpClient)
        {
            if (httpClient == null)
                throw new Exception("");

            _httpClient = httpClient;
        }

        /// <summary>
        /// Requires Https secure channel for sending requests.. This is turned ON by default for security reasons. It is RECOMMENDED that you do not allow retrieval from http addresses by default.
        /// </summary>
        public bool RequireHttps { get; set; } = true;

        /// <summary>
        /// Returns a task which contains a string converted from remote document when completed, by using the provided address.
        /// </summary>
        /// <param name="address">Location of document</param>
        /// <param name="cancel">A cancellation token that can be used by other objects or threads to receive notice of cancellation. <see cref="CancellationToken"/></param>
        /// <returns>Document as a string</returns>
        public async Task<string> GetDocumentAsync(string address, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentNullException(nameof(address));

            if (!Utility.IsHttps(address) && RequireHttps)
                throw new Exception("");

            Exception unsuccessfulHttpResponseException;
            try
            {
                var httpClient = _httpClient ?? _defaultHttpClient;
                var uri = new Uri(address, UriKind.RelativeOrAbsolute);
                var response = await httpClient.GetAsync(uri, cancel).ConfigureAwait(false);
                var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                    return responseContent;
                 unsuccessfulHttpResponseException = new IOException();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new Exception("");
        }
    }
}
