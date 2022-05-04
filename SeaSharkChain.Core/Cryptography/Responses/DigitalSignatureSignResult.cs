namespace SeaSharkChain.Core;
public record DigitalSignatureSignResult
{
    public byte[] SignatureBytes { get; }
    public string SignatureTextIn64Base => Convert.ToBase64String(SignatureBytes);

    public DigitalSignatureSignResult(byte[] signature)
    {
        SignatureBytes = signature;
    }
}

