using FluentAssertions;
using NSubstitute;
using OctoCrypto.Core.Exchange;
using OctoCrypto.Core.SymbolSummary;
using OctoCrypto.Tests.Fakes;

namespace OctoCrypto.Tests;

public class SymbolSummaryJobTests
{
    [Fact]
    public async Task Symbol_summaries_are_saved_in_cache()
    {
        // arrange
        List<SymbolSummary> summaries =
        [
            new()
            {
                Symbol = "MEMEUSDT",
                PriceTickers =
                [
                    PriceTicker.WithBestAsk(Exchange.Mexc, 0.0141m, 0.0142m),
                    PriceTicker.Create(Exchange.Gate, 0.0140m, 0.0143m),
                    PriceTicker.WithBestBid(Exchange.BingX, 0.0142m, 0.0144m)
                ]
            },

            new()
            {
                Symbol = "BTCUSDT",
                PriceTickers =
                [
                    PriceTicker.WithBestBid(Exchange.Mexc, 99774.2m, 99774.3m),
                    PriceTicker.Create(Exchange.Gate, 99774.1m, 99774.2m),
                    PriceTicker.WithBestAsk(Exchange.BingX, 99774.0m, 99774.1m)
                ]
            }
        ];
        var cache = new FakeSymbolSummaryCache();
        var mexcExchangeApi = CreateExchangeApi([
            Ticker.Create("MEMEUSDT", 0.0141m, 0.0142m),
            Ticker.Create("BTCUSDT", 99774.2m, 99774.3m)
        ], Exchange.Mexc);
        var gateExchangeApi = CreateExchangeApi([
            Ticker.Create("MEMEUSDT", 0.0140m, 0.0143m),
            Ticker.Create("BTCUSDT", 99774.1m, 99774.2m)
        ], Exchange.Gate);
        var bingxExchangeApi = CreateExchangeApi([
            Ticker.Create("MEMEUSDT", 0.0142m, 0.0144m),
            Ticker.Create("BTCUSDT", 99774.0m, 99774.1m)
        ], Exchange.BingX);
        var job = new SymbolSummaryJob((List<IExchangeApi>) [mexcExchangeApi, gateExchangeApi, bingxExchangeApi],
            cache);

        // act
        await job.Execute(null!);

        // assert
        var savedSummaries = await cache.GetSummaries();
        savedSummaries.Should().BeEquivalentTo(summaries);
    }

    [Fact]
    public async Task Cached_symbol_summaries_are_ordered_by_tickers_count()
    {
        // arrange
        List<SymbolSummary> summaries =
        [
            new()
            {
                Symbol = "MEMEUSDT",
                PriceTickers =
                [
                    PriceTicker.WithBestAsk(Exchange.Mexc, 0.0141m, 0.0142m),
                    PriceTicker.Create(Exchange.Gate, 0.0140m, 0.0143m),
                    PriceTicker.WithBestBid(Exchange.BingX, 0.0142m, 0.0144m)
                ]
            },
            new()
            {
                Symbol = "BTCUSDT",
                PriceTickers =
                [
                    PriceTicker.WithBestBid(Exchange.Mexc, 99774.2m, 99774.3m),
                    PriceTicker.WithBestAsk(Exchange.BingX, bestBid: 99774.0m, bestAsk: 99774.1m)
                ]
            },
            new()
            {
                Symbol = "MAKUSDT",
                PriceTickers = [PriceTicker.WithBestSpread(Exchange.Gate, 0.03001m, 0.03021m)]
            }
        ];
        var cache = new FakeSymbolSummaryCache();
        var mexcExchangeApi = CreateExchangeApi([
            Ticker.Create("MEMEUSDT", 0.0141m, 0.0142m),
            Ticker.Create("BTCUSDT", 99774.2m, 99774.3m)
        ], Exchange.Mexc);
        var gateExchangeApi = CreateExchangeApi([
            Ticker.Create("MEMEUSDT", 0.0140m, 0.0143m),
            Ticker.Create("MAKUSDT", 0.03001m, 0.03021m),
        ], Exchange.Gate);
        var bingxExchangeApi = CreateExchangeApi([
            Ticker.Create("MEMEUSDT", 0.0142m, 0.0144m),
            Ticker.Create("BTCUSDT", 99774.0m, 99774.1m)
        ], Exchange.BingX);
        var job = new SymbolSummaryJob((List<IExchangeApi>) [mexcExchangeApi, gateExchangeApi, bingxExchangeApi],
            cache);

        // act
        await job.Execute(null!);

        // assert
        var savedSummaries = await cache.GetSummaries();
        savedSummaries.Should().HaveCount(summaries.Count);
        // RESEARCH: find a way to compare ordered collections
        for (var i = 0; i < savedSummaries.Count; i++)
        {
            savedSummaries.ElementAt(i).Should().BeEquivalentTo(summaries.ElementAt(i));
        }
    }

    private static IExchangeApi CreateExchangeApi(List<Ticker> tickers,
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