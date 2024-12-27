namespace OctoCrypto.Core.SymbolSummary;

public class PriceTicker
{
    public PriceTicker()
    {
    }

    private PriceTicker(
        Exchange.Exchange exchange,
        decimal bestBid,
        decimal bestAsk,
        bool? isBestBid = null,
        bool? isBestAsk = null) : this()
    {
        Exchange = exchange;
        BestBid = bestBid;
        BestAsk = bestAsk;
        IsBestBid = isBestBid;
        IsBestAsk = isBestAsk;
    }

    public static PriceTicker Create(Exchange.Exchange exchange, decimal bestBid, decimal bestAsk, bool? isBestBid = null, bool? isBestAsk = null)
    {
        return new PriceTicker(exchange, bestBid, bestAsk, isBestBid, isBestAsk);
    }
    
    public static PriceTicker WithBestBid(Exchange.Exchange exchange, decimal bestBid, decimal bestAsk)
    {
        return new PriceTicker(exchange, bestBid, bestAsk, true, null);
    }
    
    public static PriceTicker WithBestAsk(Exchange.Exchange exchange, decimal bestBid, decimal bestAsk)
    {
        return new PriceTicker(exchange, bestBid, bestAsk, null, true);
    }

    public static PriceTicker WithBestSpread(Exchange.Exchange exchange, decimal bestBid, decimal bestAsk)
    {
        return new PriceTicker(exchange, bestBid, bestAsk, true, true);
    }

    public Exchange.Exchange Exchange { get; set; }
    public decimal BestBid { get; set; }
    public decimal BestAsk { get; set; }
    public bool? IsBestBid { get; set; }
    public bool? IsBestAsk { get; set; }
}