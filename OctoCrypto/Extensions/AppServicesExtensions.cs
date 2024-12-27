using OctoCrypto.Cache;
using OctoCrypto.Core.Exchange;
using OctoCrypto.Core.SymbolSummary;
using OctoCrypto.ExchangeApis.BingX;
using OctoCrypto.ExchangeApis.Gate;
using OctoCrypto.ExchangeApis.Mexc;

namespace OctoCrypto.Extensions;

public static class AppServicesExtensions
{
    public static IServiceCollection AddAppServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddMemoryCache()
            .AddSingleton<IExchangeApi, BingXApi>()
            .AddSingleton<IExchangeApi, GateApi>()
            .AddSingleton<IExchangeApi, MexcApi>()
            .AddSingleton<SymbolSummaryJob>()
            .AddSingleton<SymbolSummaryService>()
            .AddSingleton<ISymbolSummaryCache, SymbolSummaryCache>();
        
        return serviceCollection;
    }
}