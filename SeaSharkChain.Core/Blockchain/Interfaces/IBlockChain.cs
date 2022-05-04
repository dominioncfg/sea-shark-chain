namespace SeaSharkChain.Core;

public interface IBlockChain<T>
{
    Task AcceptBlock(IEnumerable<ITransaction<T>> block, CancellationToken cancellationToken);
    Task<bool> Verify(CancellationToken cancellationToken);
}
