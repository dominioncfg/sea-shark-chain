using System.Security.Cryptography;

namespace SeaSharkChain.Core;

public static class RsaDigitalSignature
{
    public static DigitalSignatureKeyPairResult CreateKeyPair(int keyLengthInBytes)
    {
        using var rsa = RSA.Create(keyLengthInBytes);
        var privateKey = rsa.ExportRSAPrivateKey();
        var publicKey = rsa.ExportRSAPublicKey();
        return new DigitalSignatureKeyPairResult(privateKey, publicKey);
    }

    public static DigitalSignatureSignResult SignHashedStringIn64Base(string hashedDataIn64Base, byte[] privateKey)
    {
        var hashBytes = Convert.FromBase64String(hashedDataIn64Base);
        return SignHashed(hashBytes, privateKey);
    }

    public static DigitalSignatureSignResult SignHashed(byte[] hashedData, byte[] privateKey)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPrivateKey(privateKey, out _);

        var signature = rsa.SignHash(hashedData, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

        return new DigitalSignatureSignResult(signature);
    }

    public static DigitalVerifyResult VerifyHashedStringIn64Base(string signatureIn64Base, byte[] publicKey, string hashOfDataIn64Base)
    {
        var signatureBytes = Convert.FromBase64String(signatureIn64Base);
        var hashBytes = Convert.FromBase64String(hashOfDataIn64Base);
        return VerifyHashed(signatureBytes, publicKey, hashBytes);
    }

    public static DigitalVerifyResult VerifyHashed(byte[] signature, byte[] publicKey, byte[] hashOfData)
    {
        using var rsa = RSA.Create();
        rsa.ImportRSAPublicKey(publicKey, out _);

        var result = rsa.VerifyHash(hashOfData, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);

        return new DigitalVerifyResult(result);
    }
}

