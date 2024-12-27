namespace OctoCrypto.Core.Exchange;

public class Ticker
{
    public Ticker()
    {
    }

    private Ticker(string symbol, decimal bestBid, decimal bestAsk)
    {
        Symbol = symbol;
        BestBid = bestBid;
        BestAsk = bestAsk;
    }

    public static Ticker Create(string symbol, decimal bestBid, decimal bestAsk)
    {
        return new Ticker(symbol, bestBid, bestAsk);
    }

    public string Symbol { get; set; }
    public decimal BestBid { get; set; }    
    public decimal BestAsk { get; set; }
}