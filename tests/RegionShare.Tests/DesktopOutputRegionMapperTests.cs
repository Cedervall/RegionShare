using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class DesktopOutputRegionMapperTests
{
    [Fact]
    public void MapReturnsOutputRelativeRegionForContainingOutput()
    {
        var outputs = new[]
        {
            new DesktopOutputBounds(-1920, 0, 0, 1080),
            new DesktopOutputBounds(0, 0, 2560, 1440)
        };

        var mapped = DesktopOutputRegionMapper.Map(new CaptureRegion(100, 200, 1280, 720), outputs);

        Assert.NotNull(mapped);
        Assert.Equal(1, mapped.OutputIndex);
        Assert.Equal(new CaptureRegion(100, 200, 1280, 720), mapped.RelativeRegion);
    }

    [Fact]
    public void MapHandlesNegativeCoordinateOutput()
    {
        var outputs = new[]
        {
            new DesktopOutputBounds(-1920, 0, 0, 1080),
            new DesktopOutputBounds(0, 0, 2560, 1440)
        };

        var mapped = DesktopOutputRegionMapper.Map(new CaptureRegion(-1820, 100, 640, 360), outputs);

        Assert.NotNull(mapped);
        Assert.Equal(0, mapped.OutputIndex);
        Assert.Equal(new CaptureRegion(100, 100, 640, 360), mapped.RelativeRegion);
    }

    [Fact]
    public void MapReturnsNullWhenRegionSpansOutputs()
    {
        var outputs = new[]
        {
            new DesktopOutputBounds(-1920, 0, 0, 1080),
            new DesktopOutputBounds(0, 0, 2560, 1440)
        };

        var mapped = DesktopOutputRegionMapper.Map(new CaptureRegion(-100, 100, 300, 200), outputs);

        Assert.Null(mapped);
    }
}
