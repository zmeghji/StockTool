using AlphaVantageClient.Configuration;
using AlphaVantageClient.Constants;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlphaVantageClient.ApiServices
{
    public abstract class BaseApiService<T>
    {
        protected readonly HttpClient _httpClient;
        private readonly IOptions<AlphaVantageAuthenticationOptions> _authOptions;
        private readonly string _queryStringTemplate;

        public BaseApiService(HttpClient httpClient, IOptions<AlphaVantageAuthenticationOptions> authOptions, string queryStringTemplate)
        {
            _queryStringTemplate = queryStringTemplate;
            _httpClient = httpClient;
            _authOptions = authOptions;
            _httpClient.BaseAddress = new Uri(ApiConstants.BaseUrl);
        }

        public async Task<T> Get(string stockSymbol)
        {

            var resp = await _httpClient.GetAsync(string.Format(_queryStringTemplate, stockSymbol, _authOptions.Value.ApiKey));
            var contentJsonString = await resp.Content.ReadAsStringAsync();
            return (JsonConvert.DeserializeObject<T>(contentJsonString));
        }
    }
}
