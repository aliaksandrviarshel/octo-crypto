using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using OctoCrypto.Configuration;
using WireMock.Server;

namespace OctoCrypto.Tests.Integration.WebApplicationFactory;

public class IntegrationTestsWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var server = WireMockServer.Start();

        builder.ConfigureTestServices(services =>
        {
            services
                .Configure<MexcApiOptions>(options => { options.Url = server.Urls[0]; })
                .Configure<GateApiOptions>(options => { options.Url = server.Urls[0]; })
                .Configure<BingXApiOptions>(options => { options.Url = server.Urls[0]; });
        });

        builder.ConfigureServices(services =>
        {
            services
                .AddSingleton(server);
        });
    }
}