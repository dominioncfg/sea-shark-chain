using FluentAssertions;
using SeaSharkChain.Core;
using Xunit;

namespace SeaSharkChain.Tests;

public class WhenVerifyingChains : BlockChainsTestBase
{
    [Fact]
    public async Task VerifyReturnsTrueForChainWithSingleBlock()
    {
        var settings = new BlockChainSettings()
        {
            BlockRequiredNumberOfTransactions = 2
        };
        var firstBlockTransactions = GivenNTransactions(numberOfTransactions: 2, startTransactionNumber: 1);

        var repository = new InMemoryUsersBlocksRepository();
        var chain = new BlockChain<DummyUser>(new ClockService(), repository, GivenDefaultKeyStore(), settings);

        await chain.AcceptBlock(firstBlockTransactions, default);

        repository.GetAllBlocks().Should().HaveCount(1);
        var result = await chain.Verify(default);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task VerifyReturnsTrueForChainWithTwoBlocks()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        var result = await chain.Verify(default);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockNumberHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            block.Header.TamperBlockNumber(block.Header.Number + 1);
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockCreatedAtHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            block.Header.TamperBlockCreatedAt(block.Header.CreatedAtUtc.AddSeconds(1));
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }


    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockHashHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            var changed = block.Header.HashIn64Base.WithRandomCharacterChanged();
            block.Header.TamperHashIn64Base(changed);
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockPreviousBlockHashHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            block.Header.TamperPreviousBlockHashIn64Base(block.Header.PreviousBlockHashIn64Base!.WithRandomCharacterChanged());
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockNodeSignatureHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            block.Header.TamperBlockNodeSignature(block.Header.BlockNodeSignature.WithRandomCharacterChanged());
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockPowNonceSignatureHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            block.Header.TamperPowNonce(block.Header.PowNonce + 1);
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockFirstTransactionHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            block.Transactions.Should().HaveCount(2);
            var firstTransaction = block.Transactions[0];
            firstTransaction.TamperFirstName(firstTransaction.Value.FirstName.WithRandomCharacterChanged());
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }

    [Fact]
    public async Task VerifyReturnsFalseForChainWithTwoBlocksWhenLastBlockSecondTransactionHasBeenTampered()
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

        repository.GetAllBlocks().Should().HaveCount(2);
        repository.TamperWithBlock(2, block =>
        {
            block.Transactions.Should().HaveCount(2);
            var secondTransaction = block.Transactions[1];
            secondTransaction.TamperFirstName(secondTransaction.Value.FirstName.WithRandomCharacterChanged());
        });
        var result = await chain.Verify(default);
        result.Should().BeFalse();
    }
}
