using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using OctoCrypto.Configuration;
using OctoCrypto.Core.Exchange;
using Newtonsoft.Json;

namespace OctoCrypto.ExchangeApis.Mexc;

// IMPROVE: use Refit
// https://github.com/reactiveui/refit
public class MexcApi : IExchangeApi
{
    private readonly MexcApiOptions _options;

    public MexcApi(IOptions<MexcApiOptions> options)
    {
        _options = options.Value;
    }

    public async Task<ExchangeTickers> GetTickersAsync()
    {
        try
        {
            var api = new
            {
                uri = "/api/v3/ticker/bookTicker",
                method = "GET",
            };

            var response = DoRequest(api.uri, api.method).Result;

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var apiModel = JsonConvert.DeserializeObject<List<MexcTickersResponse>>(json);
            var result = new ExchangeTickers();
            result.Exchange = Exchange.Mexc;
            result.Tickers = apiModel.Select(ticker => new Ticker
            {
                Symbol = ticker.Symbol,
                BestBid = ticker.BidPrice ?? 0,
                BestAsk = ticker.AskPrice ?? 0
            }).ToList();

            return result;
        }
        catch (Exception e)
        {
            return ExchangeTickers.Empty(Exchange.Mexc);
        }
    }

    // IMPROVE: remove code duplication from exchange apis
    private async Task<HttpResponseMessage> DoRequest(string api, string method, object? payload = null)
    {
        var timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var parameters = $"timestamp={timestamp}";

        if (payload != null)
        {
            parameters = payload
                .GetType()
                .GetProperties()
                .Aggregate(
                    parameters,
                    (current, property) => current + $"&{property.Name}={property.GetValue(payload)}"
                );
        }

        var sign = CalculateHmacSha256(parameters, _options.ApiSecret);
        var url = $"{_options.Url}{api}";

        using var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (_, _, _, _) => true;

        var client = new HttpClient();

        // IMPROVE: move to HttpClients configuration
        client.DefaultRequestHeaders.Add("X-MEXC-APIKEY", _options.ApiKey);
        var response = method.ToUpper() switch
        {
            "GET" => await client.GetAsync(url),
            "POST" => await client.PostAsync(url, null),
            "DELETE" => await client.DeleteAsync(url),
            "PUT" => await client.PutAsync(url, null),
            _ => throw new NotSupportedException("Unsupported HTTP method: " + method)
        };

        return response;
    }

    static string CalculateHmacSha256(string input, string key)
    {
        var keyBytes = System.Text.Encoding.UTF8.GetBytes(key);
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
        using var hmac = new HMACSHA256(keyBytes);
        var hashBytes = hmac.ComputeHash(inputBytes);
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
    }
}