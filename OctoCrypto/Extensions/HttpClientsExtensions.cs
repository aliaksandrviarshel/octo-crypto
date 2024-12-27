using Microsoft.Extensions.Options;
using OctoCrypto.Configuration;

namespace OctoCrypto.Extensions;

// IMPROVE: use Refit
// RESEARCH: change base url in integration tests. For now this class is not used
// https://github.com/reactiveui/refit
public static class HttpClientsExtensions
{
    public static IServiceCollection AddHttpClients(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var provider = services.BuildServiceProvider();

        var temp = builder.Configuration.GetSection(MexcApiOptions.MexcApi);
        var mexcUrl = provider.GetRequiredService<IOptions<MexcApiOptions>>().Value.Url;
        services
            .AddHttpClient(HttpClientsNames.Mexc, httpClient => { httpClient.BaseAddress = new Uri(mexcUrl); });

        var gateUrl = provider.GetRequiredService<IOptions<GateApiOptions>>().Value.Url;
        services
            .AddHttpClient(HttpClientsNames.Gate,
                httpClient => { httpClient.BaseAddress = new Uri(gateUrl); });

        var bingXUrl = provider.GetRequiredService<IOptions<BingXApiOptions>>().Value.Url;
        services
            .AddHttpClient(HttpClientsNames.BingX,
                httpClient => { httpClient.BaseAddress = new Uri(bingXUrl); });

        return services;
    }
}