namespace OctoCrypto.ExchangeApis.BingX;

public class BingXPriceData
{
    public string Symbol { get; set; }
    public decimal? AskPrice { get; set; }
    public decimal AskQty { get; set; }
    public decimal? BidPrice { get; set; }
    public decimal BidQty { get; set; }
}