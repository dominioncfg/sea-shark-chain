namespace SeaSharkChain.Core;

public record DigitalVerifyResult
{
    public bool IsValid { get; }
    public DigitalVerifyResult(bool isValid) => IsValid = isValid;
}

