namespace SeaSharkChain.Core;

public interface IKeyStore
{
    //TODO this should Implement versioning
    byte[] AuthenticatedHashKey { get; }
    byte[] GetNodeSignPrivateKey();
    //TODO If you have multiple Nodes this should be dependant Of the node that created the Block instead of the current node.
    byte[] GetNodeSignPublicKey();
}
