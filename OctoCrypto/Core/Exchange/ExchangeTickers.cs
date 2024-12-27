namespace OctoCrypto.Core.Exchange;

public class ExchangeTickers
{
    public Exchange Exchange { get; set; }
    public ICollection<Ticker> Tickers { get; set; }

    public static ExchangeTickers Empty(Exchange exchange)
    {
        return new ExchangeTickers
        {
            Exchange = exchange,
            Tickers = new List<Ticker>()
        };
    }
}