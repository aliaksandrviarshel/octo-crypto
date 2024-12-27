using System.Net;
using System.Text.RegularExpressions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using OctoCrypto.Extensions;
using OctoCrypto.Tests.Integration.AssertionsCustomization;
using OctoCrypto.Tests.Integration.Helpers;
using OctoCrypto.Tests.Integration.WebApplicationFactory;
using Quartz;
using WireMock.Server;

namespace OctoCrypto.Tests.Integration;

public class SymbolSummaryControllerTests : IClassFixture<IntegrationTestsWebApplicationFactory>, IDisposable
{
    private readonly IntegrationTestsWebApplicationFactory _factory;
    private readonly WireMockServer _mockServer;
    private readonly ISchedulerFactory _schedulerFactory;

    public SymbolSummaryControllerTests(IntegrationTestsWebApplicationFactory factory)
    {
        _factory = factory;
        _mockServer = factory.Services.GetRequiredService<WireMockServer>();
        _schedulerFactory = factory.Services.GetRequiredService<ISchedulerFactory>();
    }

    [Fact]
    public async Task First_summaries_page_retrieved_when_summary_job_has_worked_out()
    {
        // arrange
        WireMockServerHelper.MapRequest(_mockServer, "/api/v3/ticker/bookTicker", "Data/MexcTicketsResponse.json");
        WireMockServerHelper.MapRequest(_mockServer, "/api/v4/spot/tickers", "Data/GateTicketsResponse.json");
        WireMockServerHelper.MapRequest(_mockServer, "/openApi/swap/v2/quote/ticker", "Data/BingXTicketsResponse.json");

        await TriggerJobsAndWait();
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/SymbolSummary");

        // assert
        response.Should()
            .HaveStatusCode(HttpStatusCode.OK)
            .And.HaveJsonBody("Data/DefaultSymbolSummariesResponse.json");
    }

    [Fact]
    public async Task First_summaries_page_retrieved_when_summary_job_has_worked_out_with_external_api_errors()
    {
        // arrange
        WireMockServerHelper.MapErrorRequest(_mockServer, "/api/v3/ticker/bookTicker",
            StatusCodes.Status500InternalServerError);
        WireMockServerHelper.MapRequest(_mockServer, "/api/v4/spot/tickers", "Data/GateTicketsResponse.json");
        WireMockServerHelper.MapRequest(_mockServer, "/openApi/swap/v2/quote/ticker", "Data/BingXTicketsResponse.json");

        await TriggerJobsAndWait();
        var client = _factory.CreateClient();

        // act
        var response = await client.GetAsync("/SymbolSummary");

        // assert
        response.Should()
            .HaveStatusCode(HttpStatusCode.OK)
            .And.HaveJsonBody("Data/ErrorSymbolSummariesResponse.json");
    }

    private async Task TriggerJobsAndWait()
    {
        var schedule = await _schedulerFactory.GetScheduler();
        await schedule.TriggerJob(JobKeys.SymbolSummary);
        await Task.Delay(TimeSpan.FromSeconds(1));
    }


    public void Dispose()
    {
        _mockServer.Reset();
    }
}