using OctoCrypto.Cache;
using OctoCrypto.Core.Exchange;
using Quartz;

namespace OctoCrypto.Core.SymbolSummary;

public class SymbolSummaryJob : IJob
{
    private readonly IEnumerable<IExchangeApi> _exchangeApis;
    private readonly ISymbolSummaryCache _cache;

    public SymbolSummaryJob(IEnumerable<IExchangeApi> exchangeApis, ISymbolSummaryCache cache)
    {
        _exchangeApis = exchangeApis;
        _cache = cache;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var getTickersTasks = _exchangeApis.Select(api => api.GetTickersAsync()).ToList();
        var exchangesTickers = await Task.WhenAll(getTickersTasks);

        var symbolSummaries = exchangesTickers
            .SelectMany(tickers => tickers.Tickers.Select(ticker => (ticker, tickers.Exchange)))
            .GroupBy(ticker => ticker.ticker.Symbol)
            .Select(tickersGroup =>
            {
                var bestAsk = tickersGroup.Min(ticket => ticket.ticker.BestAsk);
                var bestBid = tickersGroup.Max(ticket => ticket.ticker.BestBid);
                return new SymbolSummary
                {
                    Symbol = tickersGroup.Key,
                    PriceTickers = tickersGroup.Select(ticker => new PriceTicker
                    {
                        Exchange = ticker.Exchange,
                        BestAsk = ticker.ticker.BestAsk,
                        BestBid = ticker.ticker.BestBid,
                        IsBestAsk = ticker.ticker.BestAsk == bestAsk ? true : null,
                        IsBestBid = ticker.ticker.BestBid == bestBid ? true : null,
                    }).ToList()
                };
            })
            .ToList();

        await _cache.SaveSummaries(symbolSummaries);
    }
}