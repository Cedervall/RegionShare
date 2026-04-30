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
}
