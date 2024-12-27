using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace OctoCrypto.Tests.Integration.Helpers;

public static class WireMockServerHelper
{
    public static void MapRequest(WireMockServer server, string requestPath, string responseFilePath)
    {
        server
            .Given(
                Request.Create()
                    .WithPath(requestPath)
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithBodyFromFile(responseFilePath)
            );
    }

    public static void MapErrorRequest(WireMockServer server, string requestPath, int code)
    {
        server
            .Given(
                Request.Create()
                    .WithPath(requestPath)
                    .UsingGet()
            )
            .RespondWith(
                Response.Create()
                    .WithStatusCode(code)
            );
    }
}