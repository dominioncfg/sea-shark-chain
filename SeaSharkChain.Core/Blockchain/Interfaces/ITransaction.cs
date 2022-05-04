namespace SeaSharkChain.Core;

public interface ITransaction<T>
{
    T Value { get; }
    string CalculateHashIn64Base(byte[] authenticatedHashKey);
}
