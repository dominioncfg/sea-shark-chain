namespace SeaSharkChain.Core;

public record BlockCreationArgs<T>
{
    public int BlockNumber { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public IEnumerable<ITransaction<T>> Transactions { get; init; } = Array.Empty<ITransaction<T>>();
    public string? ParentBlockHash { get; init; }
    public byte[] AuthenticatedHashKey { get; init; } = Array.Empty<byte>();
    public byte[] CreatorNodeSigningKey { get; init; } = Array.Empty<byte>();
    public int MiningDifficuly { get; init; }
}
