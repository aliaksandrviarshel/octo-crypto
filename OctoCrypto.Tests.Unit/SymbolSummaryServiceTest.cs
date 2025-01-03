using FluentAssertions;
using OctoCrypto.Core.SymbolSummary;
using OctoCrypto.Tests.Fakes;
using OctoCrypto.Tests.Helpers;

namespace OctoCrypto.Tests;

public class SymbolSummaryServiceTest
{
    [Theory]
    [InlineData(20, 1, 10, 10)]
    [InlineData(20, 2, 5, 5)]
    [InlineData(20, 3, 9, 2)]
    [InlineData(20, 3, 15, 0)]
    public async Task Specified_page_is_retrieved(int allSummaries, int pageIndex, int pageSize,
        int expectedCount)
    {
        // arrange
        var summaries = SymbolSummaryFactory.CreateRandomFakes(allSummaries);
        var cache = new FakeSymbolSummaryCache();
        var service = new SymbolSummaryService(cache);
        await cache.SaveSummaries(summaries);

        // act
        var retrievedSummaries = await service.GetSymbolSummaries(pageIndex, pageSize);

        // assert
        retrievedSummaries.Should().HaveCount(expectedCount);
    }

    [Theory]
    [InlineData(20, 10, 10)]
    [InlineData(20, 15, 15)]
    public async Task First_page_is_retrieved_when_page_index_is_not_provided(
        int allSummaries,
        int pageSize,
        int expectedCount)
    {
        // arrange
        var summaries = SymbolSummaryFactory.CreateRandomFakes(allSummaries);
        var cache = new FakeSymbolSummaryCache();
        var service = new SymbolSummaryService(cache);
        await cache.SaveSummaries(summaries);

        // act
        var retrievedSummaries = await service.GetSymbolSummaries(pageSize: pageSize);

        // assert
        retrievedSummaries.Should().HaveCount(expectedCount);
    }

    [Theory]
    [InlineData(20, 1, 10)]
    [InlineData(20, 2, 10)]
    public async Task Ten_Summaries_are_retrieved_when_page_size_is_not_provided(
        int allSummaries,
        int pageIndex,
        int expectedCount)
    {
        // arrange
        var summaries = SymbolSummaryFactory.CreateRandomFakes(allSummaries);
        var cache = new FakeSymbolSummaryCache();
        var service = new SymbolSummaryService(cache);
        await cache.SaveSummaries(summaries);

        // act
        var retrievedSummaries = await service.GetSymbolSummaries(pageIndex: pageIndex);

        // assert
        retrievedSummaries.Should().HaveCount(expectedCount);
    }
}