using OctoCrypto.Cache;

namespace OctoCrypto.Core.SymbolSummary;

public class SymbolSummaryService
{
    private readonly ISymbolSummaryCache _symbolSummaryCache;

    public SymbolSummaryService(ISymbolSummaryCache symbolSummaryCache)
    {
        _symbolSummaryCache = symbolSummaryCache;
    }

    public async Task<ICollection<SymbolSummary>> GetSymbolSummaries(int? pageIndex = 1, int? pageSize = 10)
    {
        var resolvedPageIndex = pageIndex ?? 1;
        var resolvedPageSize = pageSize ?? 10;
        
        var allSummaries = await _symbolSummaryCache.GetSummaries();

        var summaries = allSummaries
            .OrderByDescending(summary => summary.PriceTickers.Count)
            .ThenBy(summary => summary.Symbol)
            .Skip((resolvedPageIndex - 1) * resolvedPageSize)
            .Take(resolvedPageSize)
            .ToList();

        return summaries;
    }
}