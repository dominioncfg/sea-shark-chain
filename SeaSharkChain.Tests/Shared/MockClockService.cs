using SeaSharkChain.Core;

namespace SeaSharkChain.Tests;

public class MockClockService: IClockService
{
    private DateTime _now;
    public DateTime NowUtc => _now;

    public MockClockService WithNow(DateTime newNow)
    {
        _now = newNow;
        return this;
    }
}