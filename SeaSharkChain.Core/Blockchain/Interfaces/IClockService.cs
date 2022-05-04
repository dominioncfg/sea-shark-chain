namespace SeaSharkChain.Core;

public interface IClockService
{
    DateTime NowUtc { get; }
}
