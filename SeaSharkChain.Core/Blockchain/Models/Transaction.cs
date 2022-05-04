namespace SeaSharkChain.Core;

public class Transaction<T> : ITransaction<T>
{
    public T Value { get; }
    public Transaction(T value) => Value = value;

    public string CalculateHashIn64Base(byte[] authenticatedHashKey)
    {
        var toHash = Value ?? throw new NullReferenceException("Empty Transaction");
        return HmacHasher.ComputeHmacSha2_512(toHash, authenticatedHashKey).StringIn64Base;
    }
}
