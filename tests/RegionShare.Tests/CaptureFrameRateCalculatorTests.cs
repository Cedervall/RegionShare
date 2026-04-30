using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class CaptureFrameRateCalculatorTests
{
    [Theory]
    [InlineData(30)]
    [InlineData(60)]
    [InlineData(90)]
    [InlineData(120)]
    public void SanitizeKeepsSupportedFrameRates(int framesPerSecond)
    {
        Assert.Equal(framesPerSecond, CaptureFrameRateCalculator.Sanitize(framesPerSecond));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(59)]
    [InlineData(144)]
    public void SanitizeFallsBackToDefaultForUnsupportedFrameRates(int framesPerSecond)
    {
        Assert.Equal(60, CaptureFrameRateCalculator.Sanitize(framesPerSecond));
    }

    [Theory]
    [InlineData(30, 333333)]
    [InlineData(60, 166666)]
    [InlineData(90, 111111)]
    [InlineData(120, 83333)]
    public void ToIntervalMapsFrameRatesToTimerIntervals(int framesPerSecond, long expectedTicks)
    {
        Assert.Equal(TimeSpan.FromTicks(expectedTicks), CaptureFrameRateCalculator.ToInterval(framesPerSecond));
    }
}
