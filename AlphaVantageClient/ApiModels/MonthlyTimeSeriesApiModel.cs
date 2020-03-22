using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaVantageClient.ApiModels
{
    public class MonthlyTimeSeriesApiModel
    {
        [JsonProperty("Monthly Time Series")]
        public IDictionary<string, MonthQuoteApiModel> MonthlyTimeSeries { get; set; }
    }
    
    public class MonthQuoteApiModel
    {
        [JsonProperty("3. low")]
        public double Low { get; set; }

        [JsonProperty("4. close")]
        public double Close { get; set; }
        
    }
}
