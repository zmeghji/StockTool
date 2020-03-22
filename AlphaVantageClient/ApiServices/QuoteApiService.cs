using AlphaVantageClient.ApiModels;
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
    public interface IQuoteApiService
    {
        Task<QuoteWrapperApiModel> Get(string symbol);
    }
    
    public class QuoteApiService : BaseApiService<QuoteWrapperApiModel>, IQuoteApiService
    {
        public QuoteApiService(HttpClient httpClient, IOptions<AlphaVantageAuthenticationOptions> authOptions): 
            base(httpClient, authOptions, "?function=GLOBAL_QUOTE&symbol={0}&apikey={1}") {}
    }
}
