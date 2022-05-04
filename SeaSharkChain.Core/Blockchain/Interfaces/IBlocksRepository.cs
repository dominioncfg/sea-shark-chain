namespace SeaSharkChain.Core;

public interface IBlocksRepository<T>
{
    public Task Add(Block<T> block, CancellationToken cancellationToken);
    public Task<IEnumerable<Block<T>>> GetBatch(int blockNumber, int count, CancellationToken cancellationToken);
}
