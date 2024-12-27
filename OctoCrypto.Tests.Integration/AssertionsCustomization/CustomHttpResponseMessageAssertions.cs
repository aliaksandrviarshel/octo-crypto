using System.Text.RegularExpressions;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace OctoCrypto.Tests.Integration.AssertionsCustomization;

public static class CustomHttpResponseMessageAssertions
{
    public static AndConstraint<HttpResponseMessageAssertions> HaveJsonBody(
        this HttpResponseMessageAssertions responseMessageAssertions,
        string pathToExpected,
        string because = "",
        params object[] becauseArgs)
    {
        var expectedResponse = File.ReadAllText(pathToExpected);
        // remove spacing caused by formatting in file
        expectedResponse = new Regex(@"\s+").Replace(expectedResponse, string.Empty);

        var content = responseMessageAssertions.Subject.Content.ReadAsStringAsync().Result;

        content.Should().Be(expectedResponse, because, becauseArgs);

        return new AndConstraint<HttpResponseMessageAssertions>(responseMessageAssertions);
    }
}