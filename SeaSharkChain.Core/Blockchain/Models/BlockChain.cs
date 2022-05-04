﻿namespace SeaSharkChain.Core;

public class BlockChain<T> : IBlockChain<T>
{
    private const int BlockRequiredNumberOfTransactions = 2;

    private readonly IClockService _clockService;
    private readonly IBlocksRepository<T> _blocksRepository;
    private readonly IKeyStore _keyStore;

    private IBlock<T>? Genesis { get; set; }
    private IBlock<T>? CurrentBlock { get; set; }

    public BlockChain(IClockService clockService, IBlocksRepository<T> blocksRepository, IKeyStore keyStore)
    {
        _clockService = clockService;
        _blocksRepository = blocksRepository;
        _keyStore = keyStore;
    }

    public async Task AcceptBlock(IEnumerable<ITransaction<T>> blockTransactions, CancellationToken cancellationToken)
    {
        var transactions = blockTransactions.ToList();
        if (transactions.Count != BlockRequiredNumberOfTransactions)
            throw new Exception($"A block with {transactions.Count} cannot be created. You need to supply {BlockRequiredNumberOfTransactions} transactions.");

        var blockNumber = CurrentBlock?.Header.Number + 1 ?? 1;
        var createdAt = _clockService.NowUtc;
        var parentHash = CurrentBlock?.Header.HashIn64Base;
        var newBlockArgs = new BlockCreationArgs<T>()
        {
            BlockNumber = blockNumber,
            CreatedAtUtc = createdAt,
            Transactions = transactions,
            ParentBlockHash = parentHash,
            AuthenticatedHashKey = _keyStore.AuthenticatedHashKey,
            CreatorNodeSigningKey = _keyStore.GetNodeSignPrivateKey(),
        };
        var block = new Block<T>(newBlockArgs);

        await _blocksRepository.Add(block, cancellationToken);

        if (Genesis is null)
            Genesis = block;

        CurrentBlock = block;
    }

    public async Task<bool> Verify(CancellationToken cancellationToken)
    {
        int blockNumber = 0;
        IEnumerable<Block<T>> blocks;
        Block<T>? lastVerifiedBlock = null;
        do
        {
            blocks = await _blocksRepository.GetBatch(blockNumber + 1, 1000, cancellationToken);
            foreach (var block in blocks)
            {
                //TODO Creator Node != this Node
                var creatorPublicKey = _keyStore.GetNodeSignPublicKey();
                bool isValid = block.Verify(_keyStore.AuthenticatedHashKey, creatorPublicKey, lastVerifiedBlock?.Header.HashIn64Base);
                if (!isValid)
                    return false;

                lastVerifiedBlock = block;
                blockNumber = block.Header.Number;
            }

        } while (blocks.Any());
        return true;
    }
}