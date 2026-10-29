using Sensus.Application.Common.Interfaces;

namespace Sensus.TestKit.Fakes;

public sealed class FakeDateTimeProvider : IDateTimeProvider
{
    private DateTime _utcNow;

    public FakeDateTimeProvider(DateTime? initialUtc = null)
    {
        _utcNow = initialUtc ?? DateTime.UtcNow;
    }

    public DateTime UtcNow => _utcNow;

    public void Advance(TimeSpan timeSpan)
    {
        _utcNow = _utcNow.Add(timeSpan);
    }

    public void Set(DateTime utcTime)
    {
        _utcNow = DateTime.SpecifyKind(utcTime, DateTimeKind.Utc);
    }
}
