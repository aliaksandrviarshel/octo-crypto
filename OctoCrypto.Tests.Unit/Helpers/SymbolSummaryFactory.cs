using Bogus;
using OctoCrypto.Core.Exchange;
using OctoCrypto.Core.SymbolSummary;

namespace OctoCrypto.Tests.Helpers;

public static class SymbolSummaryFactory
{
    public static List<SymbolSummary> CreateRandomFakes(int count)
    {
        var priceTickersFake = new Faker<PriceTicker>()
            .RuleFor(pt => pt.Exchange, f => f.PickRandom<Exchange>())
            .RuleFor(pt => pt.BestBid, f => f.Random.Decimal())
            .RuleFor(pt => pt.BestAsk, f => f.Random.Decimal());
        
        var summariesFake = new Faker<SymbolSummary>()
            .RuleFor(s => s.Symbol, f => f.Random.String(6))
            .RuleFor(s => s.PriceTickers, () =>
            {
                var tickers = priceTickersFake.Generate(1);
                var firstTicker = tickers.FirstOrDefault();
                if (firstTicker != null)
                {
                    firstTicker.IsBestAsk = true;
                    firstTicker.IsBestBid = true;
                }

                return tickers;
            });

        return summariesFake.Generate(count);
    }
}