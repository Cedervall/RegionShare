using System.Windows;
using RegionShare.App.Dpi;

namespace RegionShare.Tests;

public sealed class DpiServiceTests
{
    [Fact]
    public void ToPhysicalRegionScalesLogicalCoordinates()
    {
        var service = new DpiService();

        var region = service.ToPhysicalRegion(new Rect(10, 20, 100, 50), 1.5, 2.0);

        Assert.Equal(15, region.X);
        Assert.Equal(40, region.Y);
        Assert.Equal(150, region.Width);
        Assert.Equal(100, region.Height);
    }

    [Theory]
    [InlineData(1.0, 100, 50)]
    [InlineData(1.25, 125, 63)]
    [InlineData(1.5, 150, 75)]
    [InlineData(2.0, 200, 100)]
    public void ToPhysicalRegionHandlesCommonDpiScales(double scale, int expectedWidth, int expectedHeight)
    {
        var service = new DpiService();

        var region = service.ToPhysicalRegion(new Rect(0, 0, 100, 50), scale, scale);

        Assert.Equal(expectedWidth, region.Width);
        Assert.Equal(expectedHeight, region.Height);
    }

    [Fact]
    public void ToPhysicalRegionPreservesNegativeMonitorCoordinates()
    {
        var service = new DpiService();

        var region = service.ToPhysicalRegion(new Rect(-1280, -100, 640, 360), 1.5, 1.25);

        Assert.Equal(-1920, region.X);
        Assert.Equal(-125, region.Y);
        Assert.Equal(960, region.Width);
        Assert.Equal(450, region.Height);
    }

    [Fact]
    public void ToPhysicalRegionRoundsMidpointsAwayFromZero()
    {
        var service = new DpiService();

        var region = service.ToPhysicalRegion(new Rect(-10.5, -20.5, 100.5, 50.5), 1.0, 1.0);

        Assert.Equal(-11, region.X);
        Assert.Equal(-21, region.Y);
        Assert.Equal(101, region.Width);
        Assert.Equal(51, region.Height);
    }

    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    [InlineData(-1, 1)]
    [InlineData(1, -1)]
    public void ToPhysicalRegionRejectsInvalidDpiScales(double scaleX, double scaleY)
    {
        var service = new DpiService();

        Assert.Throws<ArgumentOutOfRangeException>(() => service.ToPhysicalRegion(new Rect(0, 0, 100, 50), scaleX, scaleY));
    }
}
