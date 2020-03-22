using AlphaVantageClient.ApiModels;
using AlphaVantageClient.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AlphaVantageClient.ApiServices
{
    public interface IDailyTimeSeriesApiService
    {
        Task<DailyTimeSeriesApiModel> Get(string stockSymbol);
    }
    public class DailyTimeSeriesApiService : BaseApiService<DailyTimeSeriesApiModel>, IDailyTimeSeriesApiService
    {
        public DailyTimeSeriesApiService(
            HttpClient httpClient, 
            IOptions<AlphaVantageAuthenticationOptions> authOptions) 
            : base(httpClient, authOptions, "?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}")
        {
        }
    }
}
