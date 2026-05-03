namespace RegionShare.App.Preview;

using System.Globalization;

public static class FrameTimingLabelFormatter
{
    public static string EmptyLabel { get; } = "Latency -- ms | Frame -- ms";

    public static string Format(FrameTimingSample? sample)
    {
        if (sample is null)
        {
            return EmptyLabel;
        }

        var frameInterval = sample.FrameIntervalMilliseconds is null
            ? "--"
            : FormatMilliseconds(sample.FrameIntervalMilliseconds.Value);

        return $"Latency {FormatMilliseconds(sample.LatencyMilliseconds)} ms | Frame {frameInterval} ms";
    }

    private static string FormatMilliseconds(double milliseconds)
    {
        return milliseconds < 10
            ? milliseconds.ToString("0.0", CultureInfo.InvariantCulture)
            : milliseconds.ToString("0", CultureInfo.InvariantCulture);
    }
}
