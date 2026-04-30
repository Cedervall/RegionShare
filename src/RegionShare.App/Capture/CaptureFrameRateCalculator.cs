namespace RegionShare.App.Capture;

public static class CaptureFrameRateCalculator
{
    public const int DefaultFramesPerSecond = 60;

    public static IReadOnlyList<int> SupportedFramesPerSecond { get; } = [30, 60, 90, 120];

    public static int Sanitize(int framesPerSecond)
    {
        return SupportedFramesPerSecond.Contains(framesPerSecond) ? framesPerSecond : DefaultFramesPerSecond;
    }

    public static TimeSpan ToInterval(int framesPerSecond)
    {
        var sanitizedFramesPerSecond = Sanitize(framesPerSecond);
        return TimeSpan.FromTicks(TimeSpan.TicksPerSecond / sanitizedFramesPerSecond);
    }
}
