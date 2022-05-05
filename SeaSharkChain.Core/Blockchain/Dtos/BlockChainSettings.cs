namespace SeaSharkChain.Core;

public record BlockChainSettings
{
    public int BlockRequiredNumberOfTransactions { get; init; } = 10;

    /// <summary>
    /// A positive number the bigger the more time it will take to calculate the PoW
    /// </summary>
    public int ProofOfWorkDifficulty { get; init; } = 3;
}
