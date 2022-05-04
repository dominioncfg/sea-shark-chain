namespace SeaSharkChain.Core;

public record BlockHashPayload
{
    public int Number { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public string? PreviousBlockHashIn64Base { get; init; }
    public string TransactionHash { get; init; } = string.Empty;
}