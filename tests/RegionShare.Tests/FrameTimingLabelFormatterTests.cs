using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class FrameTimingLabelFormatterTests
{
    [Fact]
    public void FormatReturnsEmptyLabelWhenNoSampleExists()
    {
        Assert.Equal("Latency -- ms | Frame -- ms", FrameTimingLabelFormatter.Format(null));
    }

    [Fact]
    public void FormatUsesMissingFrameIntervalForFirstFrame()
    {
        var label = FrameTimingLabelFormatter.Format(new FrameTimingSample(8.4, null));

        Assert.Equal("Latency 8.4 ms | Frame -- ms", label);
    }

    [Fact]
    public void FormatRoundsLargeValuesWithoutDecimals()
    {
        var label = FrameTimingLabelFormatter.Format(new FrameTimingSample(12.6, 16.7));

        Assert.Equal("Latency 13 ms | Frame 17 ms", label);
    }
}
