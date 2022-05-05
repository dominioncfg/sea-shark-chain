namespace SeaSharkChain.Core;

public record ProofOfWorkCalculateResult
{
    public int Nonce { get; init; }

    public string HashIn64Base { get; init; } = string.Empty;
}

