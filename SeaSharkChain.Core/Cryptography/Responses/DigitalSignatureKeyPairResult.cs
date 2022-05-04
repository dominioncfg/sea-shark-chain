namespace SeaSharkChain.Core;

public record DigitalSignatureKeyPairResult
{
    public byte[] PrivateKeyBytes { get; }
    public string PrivateKeyStringIn64Base => Convert.ToBase64String(PrivateKeyBytes);

    public byte[] PublicKeyBytes { get; }
    public string PublicKeyStringIn64Base => Convert.ToBase64String(PublicKeyBytes);

    public DigitalSignatureKeyPairResult(byte[] privateKey, byte[] publicKey)
    {
        PrivateKeyBytes = privateKey;
        PublicKeyBytes = publicKey;
    }
}

