using OctoCrypto.Cache;
using OctoCrypto.Core.SymbolSummary;

namespace OctoCrypto.Tests.Fakes;

public class FakeSymbolSummaryCache : ISymbolSummaryCache
{
    private ICollection<SymbolSummary> _symbolSummaryCache = new List<SymbolSummary>();

    public Task<ICollection<SymbolSummary>> GetSummaries()
    {
        return Task.FromResult(_symbolSummaryCache);
    }

    public Task<ICollection<SymbolSummary>> SaveSummaries(ICollection<SymbolSummary> summaries)
    {
        _symbolSummaryCache = summaries;
        return Task.FromResult(_symbolSummaryCache);
    }
}