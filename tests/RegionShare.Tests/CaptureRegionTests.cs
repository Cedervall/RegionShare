using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class CaptureRegionTests
{
    [Fact]
    public void IsValidReturnsTrueForPositiveDimensions()
    {
        var region = new CaptureRegion(0, 0, 1280, 720);

        Assert.True(region.IsValid);
    }

    [Fact]
    public void IsValidReturnsFalseForZeroDimensions()
    {
        var region = new CaptureRegion(0, 0, 0, 720);

        Assert.False(region.IsValid);
    }
}
