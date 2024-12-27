namespace OctoCrypto.Core.Exchange;

public interface IExchangeApi
{
    Task<ExchangeTickers> GetTickersAsync();
}
