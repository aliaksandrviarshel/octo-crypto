using Microsoft.Extensions.Caching.Memory;
using OctoCrypto.Core.SymbolSummary;

namespace OctoCrypto.Cache;

// IMPROVE: use Redis
public class SymbolSummaryCache : ISymbolSummaryCache
{
    private const string Key = "SymbolSummaryCache";
    private readonly IMemoryCache _memoryCache;

    public SymbolSummaryCache(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public Task<ICollection<SymbolSummary>> GetSummaries()
    {
        var summaries = (ICollection<SymbolSummary>)_memoryCache.Get(Key)!;
        return Task.FromResult(summaries);
    }

    public Task<ICollection<SymbolSummary>> SaveSummaries(ICollection<SymbolSummary> summaries)
    {
        var result = _memoryCache.Set(Key, summaries);
        return Task.FromResult(result);
    }
}