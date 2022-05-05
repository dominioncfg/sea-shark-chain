using System.Security.Cryptography;
using System.Text;

namespace SeaSharkChain.Core;

public static class ShaHasher
{
    public static HashResult ComputeSha2_512(string toBeHashed)
    {
        var bytes = Encoding.UTF8.GetBytes(toBeHashed);
        return ComputeSha2_512(bytes);
    }

    public static HashResult ComputeSha2_512(byte[] toBeHashed)
    {
        using var hasher = SHA512.Create();
        var hash = hasher.ComputeHash(toBeHashed);
        return new HashResult(hash);
    }
}