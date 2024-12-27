namespace OctoCrypto.Core.SymbolSummary;

public class SymbolSummary
{
    public string Symbol { get; set; }
    public ICollection<PriceTicker> PriceTickers { get; set; }
}