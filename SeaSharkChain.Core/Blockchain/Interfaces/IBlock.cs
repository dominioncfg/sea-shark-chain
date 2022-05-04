namespace SeaSharkChain.Core;

public interface IBlock<T>
{
    BlockHeader Header { get; }
    IReadOnlyList<ITransaction<T>> Transactions { get; }
    bool Verify(byte[] authenticatedHashKey, byte[] nodeVerificationPublicKey, string? recalculatedParentBlockHash = null);
}
