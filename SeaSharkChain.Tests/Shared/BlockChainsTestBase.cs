using SeaSharkChain.Core;

namespace SeaSharkChain.Tests;

public class BlockChainsTestBase
{
    protected static IKeyStore GivenDefaultKeyStore()
    {
        return new InMemoryKeyStore(CryptographicKey.CreateRandomOfBytes(16).Bytes);
    }

    protected static List<ITransaction<DummyUser>> GivenNTransactions(int numberOfTransactions, int startTransactionNumber)
    {
        var transactions = new List<ITransaction<DummyUser>>();
        for (int i = 1; i <= numberOfTransactions; i++)
        {
            var transaction = new Transaction<DummyUser>(new DummyUser()
            {
                Id = startTransactionNumber,
                FirstName = $"Fn{startTransactionNumber}",
                LastName = $"Ln{startTransactionNumber}",
            });
            transactions.Add(transaction);
            startTransactionNumber++;
        }
        return transactions;
    }    
}
