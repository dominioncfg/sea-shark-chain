namespace SeaSharkChain.Core;

public class ClockService : IClockService
{
    public DateTime NowUtc => DateTime.UtcNow;
}
