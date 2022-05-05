using SeaSharkChain.Core;
using System.Reflection;
using System.Text;

namespace SeaSharkChain.Tests;

public static class BlockchainTestExtensions
{
    public static string WithRandomCharacterChanged(this string original)
    {
        var allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        var strBuilder = new StringBuilder(original);
        var r = new Random();
        var toChangeIndex = r.Next(0, original.Length);
        char toChangeChar;
        do
        {
            toChangeChar = allowedChars[r.Next(0, allowedChars.Length)];

        } while (toChangeChar == strBuilder[toChangeIndex]);

        strBuilder[toChangeIndex] = toChangeChar;
        return strBuilder.ToString();
    }

    public static void TamperWithBlock(this InMemoryUsersBlocksRepository repo, int blockNumber, Action<Block<DummyUser>> tamperingAction)
    {
        var block = repo
            .GetAllBlocks()
            .Single(x => x.Header.Number == blockNumber);
        tamperingAction(block);
    }

    public static void TamperBlockNumber(this BlockHeader header, int newValue)
    {
        SetPrivatePropertyValue(header, nameof(BlockHeader.Number), newValue);
    }

    public static void TamperBlockCreatedAt(this BlockHeader header, DateTime newValue)
    {
        SetPrivatePropertyValue(header, nameof(BlockHeader.CreatedAtUtc), newValue);
    }

    public static void TamperHashIn64Base(this BlockHeader header, string newValue)
    {
        SetPrivatePropertyValue(header, nameof(BlockHeader.HashIn64Base), newValue);
    }

    public static void TamperPreviousBlockHashIn64Base(this BlockHeader header, string newValue)
    {
        SetPrivatePropertyValue(header, nameof(BlockHeader.PreviousBlockHashIn64Base), newValue);
    }

    public static void TamperBlockNodeSignature(this BlockHeader header, string newValue)
    {
        SetPrivatePropertyValue(header, nameof(BlockHeader.BlockNodeSignature), newValue);
    }

    public static void TamperPowNonce(this BlockHeader header, int newValue)
    {
        SetPrivatePropertyValue(header, nameof(BlockHeader.PowNonce), newValue);
    }

    public static void TamperFirstName(this ITransaction<DummyUser> transaction, string newValue)
    {
        SetPrivatePropertyValue(transaction.Value, nameof(DummyUser.FirstName), newValue);
    }

    private static void SetPrivatePropertyValue<TObject, TProperty>(this TObject obj, string propName, TProperty val)
    {
        var t = obj?.GetType() ?? throw new ArgumentNullException(nameof(obj));

        var property = t.GetProperty(propName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        if (property == null)
            throw new ArgumentOutOfRangeException(propName, string.Format("Property {0} was not found in Type {1}", propName, obj.GetType().FullName));

        if (property.SetMethod != null)
        {
            property.SetValue(obj, val);
        }
        else
        {
            var backingField = GetPropertyBackingField(obj, propName);
            backingField.SetValue(obj, val);
        }
    }

    private static FieldInfo GetPropertyBackingField(this object obj, string propName)
    {
        var field = obj.GetType().GetField($"<{propName}>k__BackingField", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        return field ?? throw new Exception("Backing field not found.");
    }
}