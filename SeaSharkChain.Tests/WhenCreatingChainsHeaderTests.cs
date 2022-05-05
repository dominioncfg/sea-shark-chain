using FluentAssertions;
using FluentAssertions.Extensions;
using SeaSharkChain.Core;
using Xunit;

namespace SeaSharkChain.Tests;

public class WhenCreatingChainsHeaderTests : BlockChainsTestBase
{
    private const int requiredNumberOfTransactions = 2;
    [Fact]
    public async Task HeaderIsCorrectForChainWithSingleBlock()
    {
        var expectedCreatedTime = new DateTime(2020, 2, 2).At(3, 00);
        var settings = new BlockChainSettings()
        {
            BlockRequiredNumberOfTransactions = requiredNumberOfTransactions
        };
        var transactions = GivenNTransactions(numberOfTransactions: 2, startTransactionNumber: 1);

        var repository = new InMemoryUsersBlocksRepository();
        var clockService = new MockClockService()
            .WithNow(expectedCreatedTime);
        var chain = new BlockChain<DummyUser>(clockService, repository, GivenDefaultKeyStore(), settings);

        await chain.AcceptBlock(transactions, default);

        var blocksCreated = repository.GetAllBlocks().ToList();
        blocksCreated.Should().HaveCount(1);
        blocksCreated.First().Transactions.Should().HaveCount(requiredNumberOfTransactions);

        var blockHeader = blocksCreated.First().Header;
        blockHeader.Number.Should().Be(1);
        blockHeader.CreatedAtUtc.Should().Be(expectedCreatedTime);
        blockHeader.HashIn64Base.Should().NotBeNullOrEmpty();
        blockHeader.PreviousBlockHashIn64Base.Should().BeNull();
        blockHeader.BlockNodeSignature.Should().NotBeNullOrEmpty();
        blockHeader.PowNonce.Should().BeGreaterThanOrEqualTo(0);
    }

    [Fact]
    public async Task CanCreateChainWithTwoBlocks()
    {
        var firstBlockCreatedTime = new DateTime(2020, 2, 2).At(3, 00);
        var secondBlockCreatedTime = firstBlockCreatedTime.AddHours(2);
        var settings = new BlockChainSettings()
        {
            BlockRequiredNumberOfTransactions = requiredNumberOfTransactions
        };
        var firstBlockTransactions = GivenNTransactions(numberOfTransactions: 2, startTransactionNumber: 1);
        var secondBlockTransactions = GivenNTransactions(numberOfTransactions: 2, startTransactionNumber: 3);

        var repository = new InMemoryUsersBlocksRepository();
        var clockService = new MockClockService()
            .WithNow(firstBlockCreatedTime);
        var chain = new BlockChain<DummyUser>(clockService, repository, GivenDefaultKeyStore(), settings);

        await chain.AcceptBlock(firstBlockTransactions, default);
        clockService.WithNow(secondBlockCreatedTime);
        await chain.AcceptBlock(secondBlockTransactions, default);

        var blocksCreated = repository.GetAllBlocks().ToList();
        blocksCreated.Should().HaveCount(2);
        blocksCreated.First().Transactions.Should().HaveCount(requiredNumberOfTransactions);
        blocksCreated.Last().Transactions.Should().HaveCount(requiredNumberOfTransactions);

        var blockHeader = blocksCreated.First().Header;
        blockHeader.Number.Should().Be(1);
        blockHeader.CreatedAtUtc.Should().Be(firstBlockCreatedTime);
        blockHeader.HashIn64Base.Should().NotBeNullOrEmpty();
        blockHeader.PreviousBlockHashIn64Base.Should().BeNull();
        blockHeader.BlockNodeSignature.Should().NotBeNullOrEmpty();
        blockHeader.PowNonce.Should().BeGreaterThanOrEqualTo(0);


        var secondBlockHeader = blocksCreated.Last().Header;
        secondBlockHeader.Number.Should().Be(2);
        secondBlockHeader.CreatedAtUtc.Should().Be(secondBlockCreatedTime);
        secondBlockHeader.HashIn64Base.Should().NotBeNullOrEmpty();
        secondBlockHeader.PreviousBlockHashIn64Base.Should().NotBeNullOrEmpty();
        secondBlockHeader.BlockNodeSignature.Should().NotBeNullOrEmpty();
        secondBlockHeader.PowNonce.Should().BeGreaterThanOrEqualTo(0);

    }
}
