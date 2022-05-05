using SeaSharkChain.Core;

namespace SeaSharkChain.Tests;

public class InMemoryUsersBlocksRepository : IBlocksRepository<DummyUser>
{
    private readonly List<Block<DummyUser>> _storage = new();

    public Task Add(Block<DummyUser> block, CancellationToken cancellationToken)
    {
        _storage.Add(block);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<Block<DummyUser>>> GetBatch(int blockNumber, int count, CancellationToken cancellationToken)
    {
        var transactions = _storage
            .Skip(blockNumber - 1)
            .Take(count)
            .Select(x => MapFromStorage(x))
            .ToList();
        return Task.FromResult<IEnumerable<Block<DummyUser>>>(transactions);
    }

    public IReadOnlyList<Block<DummyUser>> GetAllBlocks()
    {
        return _storage
            .ToList()
            .AsReadOnly();
    }


    private static Block<DummyUser> MapFromStorage(Block<DummyUser> block)
    {
        //Simulate that we are actually storing blocks
        return Block<DummyUser>.Clone(block);
    }
}
