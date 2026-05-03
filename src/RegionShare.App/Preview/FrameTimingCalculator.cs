namespace RegionShare.App.Preview;

public static class FrameTimingCalculator
{
    public static FrameTimingSample Calculate(long capturedTimestamp, long presentedTimestamp, long? previousPresentedTimestamp, long timestampFrequency)
    {
        var latency = ToMilliseconds(presentedTimestamp - capturedTimestamp, timestampFrequency);
        double? interval = previousPresentedTimestamp is null
            ? null
            : ToMilliseconds(presentedTimestamp - previousPresentedTimestamp.Value, timestampFrequency);

        return new FrameTimingSample(Math.Max(0, latency), interval is null ? null : Math.Max(0, interval.Value));
    }

    private static double ToMilliseconds(long ticks, long timestampFrequency)
    {
        return ticks * 1000.0 / timestampFrequency;
    }
}
