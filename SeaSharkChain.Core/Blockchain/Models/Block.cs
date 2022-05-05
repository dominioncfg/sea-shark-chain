namespace SeaSharkChain.Core;

public class Block<T> : IBlock<T>
{
    private readonly List<ITransaction<T>> _transactions;
    public BlockHeader Header { get; }
    public IReadOnlyList<ITransaction<T>> Transactions => _transactions.AsReadOnly();

    public Block(BlockCreationArgs<T> args)
    {
        Header = CreateHeaders(args);
        _transactions = new(args.Transactions);
    }

    public bool Verify(byte[] authenticatedHashKey, int difficulty, byte[] nodeVerificationPublicKey, string? recalculatedParentBlockHash = null)
    {
        string recalculatedBlockHash = CalculateBlockHashIn64Base(Header.Number, Header.CreatedAtUtc, Transactions, recalculatedParentBlockHash, authenticatedHashKey);
        var powIsValid = ProofOfWork.ValidateForNonce(recalculatedBlockHash, difficulty, Header.PowNonce, Header.HashIn64Base);

        if (!powIsValid)
            return false;

        if (Header.PreviousBlockHashIn64Base != recalculatedParentBlockHash)
            return false;

        return CreatorNodeSignatureIsValid(nodeVerificationPublicKey);
    }

    private static BlockHeader CreateHeaders(BlockCreationArgs<T> args)
    {
        var initialBlockHashWithoutPow = CalculateBlockHashIn64Base(args.BlockNumber, args.CreatedAtUtc, args.Transactions, args.ParentBlockHash, args.AuthenticatedHashKey);
        var powHash = ProofOfWork.Calculate(initialBlockHashWithoutPow, args.MiningDifficuly);
        var blockSignature = SignBlockHash(powHash.HashIn64Base, args.CreatorNodeSigningKey);
        return new BlockHeader(args.BlockNumber, args.CreatedAtUtc, powHash.HashIn64Base, blockSignature, powHash.Nonce, args.ParentBlockHash);
    }

    private static string CalculateBlockHashIn64Base(int blockNumber, DateTime createdAtUtc,
                                                     IEnumerable<ITransaction<T>> transactions,
                                                     string? parentBlockHash, byte[] authenticatedHashKey)
    {
        var transactionsHash = GenerateTransactionsHash(transactions, authenticatedHashKey);
        var payload = new BlockHashPayload()
        {
            Number = blockNumber,
            CreatedAtUtc = createdAtUtc,
            PreviousBlockHashIn64Base = parentBlockHash,
            TransactionHash = transactionsHash,
        };
        return HmacHasher.ComputeHmacSha2_512(payload, authenticatedHashKey).StringIn64Base;
    }

    private static string GenerateTransactionsHash(IEnumerable<ITransaction<T>> transactions, byte[] authenticatedHashKey)
    {
        //TODO Merkle Tree
        var hashes = transactions
            .Select(transaction => transaction.CalculateHashIn64Base(authenticatedHashKey))
            .ToArray();
        return HmacHasher.ComputeHmacSha2_512(string.Join(";", hashes), authenticatedHashKey).StringIn64Base;
    }

    private static string SignBlockHash(string blockHashIn64Base, byte[] privateKey)
    {
        return RsaDigitalSignature.SignHashedStringIn64Base(blockHashIn64Base, privateKey).SignatureTextIn64Base;
    }

    private bool CreatorNodeSignatureIsValid(byte[] creatorNodePublicKey)
    {
        var isValidResult = RsaDigitalSignature.VerifyHashedStringIn64Base(Header.BlockNodeSignature, creatorNodePublicKey, Header.HashIn64Base);
        return isValidResult.IsValid;
    }
}
