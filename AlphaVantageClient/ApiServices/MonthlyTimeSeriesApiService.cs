using AlphaVantageClient.ApiModels;
using AlphaVantageClient.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AlphaVantageClient.ApiServices
{
    public interface IMonthlyTimeSeriesApiService
    {

    }
    public class MonthlyTimeSeriesApiService
        : BaseApiService<MonthlyTimeSeriesApiModel>, IMonthlyTimeSeriesApiService
    {
        public MonthlyTimeSeriesApiService(
            HttpClient httpClient, 
            IOptions<AlphaVantageAuthenticationOptions> authOptions) 
            : base(httpClient, authOptions, "?function=TIME_SERIES_MONTHLY&symbol={0}&apikey={1}")
        {
        }
    }
}
