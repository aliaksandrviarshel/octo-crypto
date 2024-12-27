using OctoCrypto.Core.SymbolSummary;

namespace OctoCrypto.Cache;

public interface ISymbolSummaryCache
{
    Task<ICollection<SymbolSummary>> GetSummaries();
    Task<ICollection<SymbolSummary>> SaveSummaries(ICollection<SymbolSummary> summaries);
}