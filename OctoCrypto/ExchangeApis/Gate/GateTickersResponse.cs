using Newtonsoft.Json;

namespace OctoCrypto.ExchangeApis.Gate;

public class GateTickersResponse
{
    [JsonProperty("currency_pair")] public string CurrencyPair { get; set; }
    [JsonProperty("lowest_ask")] public decimal? LowestAsk { get; set; }
    [JsonProperty("highest_bid")] public decimal? HighestBid { get; set; }
}