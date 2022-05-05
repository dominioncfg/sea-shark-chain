namespace SeaSharkChain.Core;

public interface IBlock<T>
{
    BlockHeader Header { get; }
    IReadOnlyList<ITransaction<T>> Transactions { get; }
    bool Verify(byte[] authenticatedHashKey, int difficultry, byte[] nodeVerificationPublicKey, string? recalculatedParentBlockHash = null);
}
