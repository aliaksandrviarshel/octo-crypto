using NSubstitute;
using OctoCrypto.Core.Exchange;

namespace OctoCrypto.Tests.Helpers;

public class ExchangeApiFactory
{
    public static IExchangeApi CreateExchangeApi(List<Ticker> tickers,
        Exchange exchange)
    {
        var mexcTickersCollection = tickers.Select(ticker =>
            Ticker.Create(ticker.Symbol, ticker.BestBid, ticker.BestAsk)).ToList();
        var mexcExchangeApi = Substitute.For<IExchangeApi>();
        var mexcTickers = new ExchangeTickers
        {
            Exchange = exchange,
            Tickers = mexcTickersCollection
        };
        mexcExchangeApi.GetTickersAsync().Returns(_ => Task.FromResult(mexcTickers));

        return mexcExchangeApi;
    }
}