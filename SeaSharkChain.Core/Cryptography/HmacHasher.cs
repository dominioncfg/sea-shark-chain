using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SeaSharkChain.Core;

public static class HmacHasher
{
    public static HashResult ComputeHmacSha2_512(string toBeHashed, byte[] key)
    {
        var bytes = Encoding.UTF8.GetBytes(toBeHashed);
        return ComputeHmacSha2_512(bytes,key);
    }

    public static HashResult ComputeHmacSha2_512(object toBeHashed, byte[] key)
    {
        var json = JsonSerializer.Serialize(toBeHashed);
        return ComputeHmacSha2_512(json,key);
    }

    private static HashResult ComputeHmacSha2_512(byte[] toBeHashed, byte[] key)
    {
        using var hasher = new HMACSHA512(key);
        var hash = hasher.ComputeHash(toBeHashed);
        return new HashResult(hash);
    }
}