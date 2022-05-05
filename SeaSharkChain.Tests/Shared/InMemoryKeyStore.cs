using SeaSharkChain.Core;

namespace SeaSharkChain.Tests;

public class InMemoryKeyStore : IKeyStore
{
    private readonly byte[] _authenticatedHashKey;
    private readonly DigitalSignatureKeyPairResult _digitalSignatureKeys;
    public byte[] AuthenticatedHashKey => _authenticatedHashKey;

    public InMemoryKeyStore(byte[] authenticatedHashKey)
    {
        _authenticatedHashKey = authenticatedHashKey;
        _digitalSignatureKeys = RsaDigitalSignature.CreateKeyPair(4096);
    }

    public byte[] GetNodeSignPrivateKey() => _digitalSignatureKeys.PrivateKeyBytes;

    public byte[] GetNodeSignPublicKey() => _digitalSignatureKeys.PublicKeyBytes;
}
