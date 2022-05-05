using FluentAssertions;
using SeaSharkChain.Core;
using Xunit;

namespace SeaSharkChain.Tests;

public class WhenCreatingChainsTransactionsTests : BlockChainsTestBase
{
    [Fact]
    public async Task CanCreateChainWithSingleBlock()
    {
        var settings = new BlockChainSettings()
        {
            BlockRequiredNumberOfTransactions = 2
        };
        var transactions = GivenNTransactions(numberOfTransactions: 2, startTransactionNumber: 1);

        var repository = new InMemoryUsersBlocksRepository();
        var chain = new BlockChain<DummyUser>(new ClockService(), repository, GivenDefaultKeyStore(), settings);

        await chain.AcceptBlock(transactions, default);

        var blocksCreated = repository.GetAllBlocks().ToList();
        blocksCreated.Should().HaveCount(1);

        var blockTransactions = blocksCreated.First().Transactions;
        blockTransactions.Should().HaveCount(2);
        blockTransactions.Should().BeEquivalentTo(transactions);
    }

    [Fact]
    public async Task CanCreateChainWithTwoBlocks()
    {
        var settings = new BlockChainSettings()
        {
            BlockRequiredNumberOfTransactions = 2
        };
        var firstBlockTransactions = GivenNTransactions(numberOfTransactions: 2, startTransactionNumber: 1);
        var secondBlockTransactions = GivenNTransactions(numberOfTransactions: 2, startTransactionNumber: 3);

        var repository = new InMemoryUsersBlocksRepository();
        var chain = new BlockChain<DummyUser>(new ClockService(), repository, GivenDefaultKeyStore(), settings);

        await chain.AcceptBlock(firstBlockTransactions, default);
        await chain.AcceptBlock(secondBlockTransactions, default);

        var blocksCreated = repository.GetAllBlocks().ToList();
        blocksCreated.Should().HaveCount(2);

        var storedFirstBlockTransactions = blocksCreated.First().Transactions;
        storedFirstBlockTransactions.Should().HaveCount(2);
        storedFirstBlockTransactions.Should().BeEquivalentTo(firstBlockTransactions);

        var storedSecondBlockTransactions = blocksCreated.Last().Transactions;
        storedSecondBlockTransactions.Should().HaveCount(2);
        storedSecondBlockTransactions.Should().BeEquivalentTo(secondBlockTransactions);
    }
}
