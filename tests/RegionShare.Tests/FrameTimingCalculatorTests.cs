using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class FrameTimingCalculatorTests
{
    [Fact]
    public void CalculateReturnsLatencyWithoutIntervalForFirstFrame()
    {
        var sample = FrameTimingCalculator.Calculate(100, 250, null, 1000);

        Assert.Equal(150, sample.LatencyMilliseconds);
        Assert.Null(sample.FrameIntervalMilliseconds);
    }

    [Fact]
    public void CalculateReturnsLatencyAndFrameIntervalForSubsequentFrame()
    {
        var sample = FrameTimingCalculator.Calculate(200, 375, 250, 1000);

        Assert.Equal(175, sample.LatencyMilliseconds);
        Assert.Equal(125, sample.FrameIntervalMilliseconds);
    }

    [Fact]
    public void CalculateClampsNegativeTimingToZero()
    {
        var sample = FrameTimingCalculator.Calculate(300, 250, 300, 1000);

        Assert.Equal(0, sample.LatencyMilliseconds);
        Assert.Equal(0, sample.FrameIntervalMilliseconds);
    }
}
