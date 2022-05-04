using FluentAssertions;
using SeaSharkChain.Core;
using Xunit;

namespace SeaSharkChain.Tests
{
    public class WhenCreatingChains
    {
        [Fact]
        public async Task CanCreateChainWithSingleBlock()
        {
            var transactions = GivenNTransactions(2);

            var repository = new InMemoryUsersBlocksRepository();
            var chain = new BlockChain<DummyUser>(new ClockService(), repository, GivenDefaultKeyStore());

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
            var firstBlockTransactions = GivenNTransactions(2);
            var secondBlockTransactions = GivenNTransactions(2);

            var repository = new InMemoryUsersBlocksRepository();
            var chain = new BlockChain<DummyUser>(new ClockService(), repository, GivenDefaultKeyStore());

            await chain.AcceptBlock(firstBlockTransactions, default);
            await chain.AcceptBlock(secondBlockTransactions, default);

            var blocksCreated = repository.GetAllBlocks().ToList();
            blocksCreated.Should().HaveCount(2);

            var storedFirstBlockTransactions = blocksCreated.First().Transactions;
            storedFirstBlockTransactions.Should().HaveCount(2);
            storedFirstBlockTransactions.Should().BeEquivalentTo(firstBlockTransactions);

            var storedSecondBlockTransactions = blocksCreated.First().Transactions;
            storedSecondBlockTransactions.Should().HaveCount(2);
            storedSecondBlockTransactions.Should().BeEquivalentTo(secondBlockTransactions);
        }

        [Fact]
        public async Task VerifyReturnsTrueForChainWithSingleBlock()
        {
            var firstBlockTransactions = GivenNTransactions(2);

            var repository = new InMemoryUsersBlocksRepository();
            var chain = new BlockChain<DummyUser>(new ClockService(), repository, GivenDefaultKeyStore());

            await chain.AcceptBlock(firstBlockTransactions, default);

            var result = await chain.Verify(default);
            result.Should().BeTrue();
        }

        [Fact]
        public async Task VerifyReturnsTrueForChainWithTwoBlocks()
        {
            var firstBlockTransactions = GivenNTransactions(2);
            var secondBlockTransactions = GivenNTransactions(2);

            var repository = new InMemoryUsersBlocksRepository();
            var chain = new BlockChain<DummyUser>(new ClockService(), repository, GivenDefaultKeyStore());

            await chain.AcceptBlock(firstBlockTransactions, default);
            await chain.AcceptBlock(secondBlockTransactions, default);

            var result = await chain.Verify(default);
            result.Should().BeTrue();
        }

        private static IKeyStore GivenDefaultKeyStore()
        {
            return new InMemoryKeyStore(CryptographicKey.CreateRandomOfBytes(16).Bytes);
        }

        private static List<ITransaction<DummyUser>> GivenNTransactions(int n)
        {
            var transactions = new List<ITransaction<DummyUser>>();
            for (int i = 1; i <= n; i++)
            {
                var transaction = new Transaction<DummyUser>(new DummyUser()
                {
                    Id = 1,
                    FirstName = "Fn",
                    LastName = "Ln",
                });
                transactions.Add(transaction);
            }
            return transactions;
        }
    }
}