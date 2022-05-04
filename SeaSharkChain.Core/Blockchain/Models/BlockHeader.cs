namespace SeaSharkChain.Core;

public class BlockHeader
{
    public int Number { get; }
    public DateTime CreatedAtUtc { get; }
    public string HashIn64Base { get; }
    public string? PreviousBlockHashIn64Base { get; }
    public string BlockNodeSignature { get; }

    public BlockHeader(int number, DateTime createdAtUtc, string hashIn64Base, string blockNodeSignature, string? previousBlockHashIn64Base = null)
    {
        Number = number;
        CreatedAtUtc = createdAtUtc;
        HashIn64Base = hashIn64Base;
        BlockNodeSignature = blockNodeSignature;
        PreviousBlockHashIn64Base = previousBlockHashIn64Base;
    }
}
