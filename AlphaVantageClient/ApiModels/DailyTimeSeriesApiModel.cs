using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaVantageClient.ApiModels
{
    public class DailyTimeSeriesApiModel
    {
        [JsonProperty("Time Series (Daily)")]
        public IDictionary<string, DailyQuoteApiModel> DailyTimeSeries { get; set; }
    }

    public class DailyQuoteApiModel
    {
        [JsonProperty("1. open")]
        public double Open { get; set; }

        [JsonProperty("3. low")]
        public double Low { get; set; }

        [JsonProperty("4. close")]
        public double Close { get; set; }

    }
}
