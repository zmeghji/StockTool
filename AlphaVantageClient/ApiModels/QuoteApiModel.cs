using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlphaVantageClient.ApiModels
{
    public class QuoteApiModel
    {
        [JsonProperty("05. price")]
        public double Price { get; set; }
    }

    public class QuoteWrapperApiModel
    {
        [JsonProperty("Global Quote")]
        public QuoteApiModel Quote { get; set; }
    }

}
