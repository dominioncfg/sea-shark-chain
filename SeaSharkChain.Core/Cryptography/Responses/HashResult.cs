namespace SeaSharkChain.Core;

public record HashResult
{
    public byte[] Bytes { get; }
    public string StringIn64Base => Convert.ToBase64String(Bytes);
    public HashResult(byte[] bytes) => Bytes = bytes;
}
