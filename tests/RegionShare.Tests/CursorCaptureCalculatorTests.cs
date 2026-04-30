using System.Windows;
using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class CursorCaptureCalculatorTests
{
    [Fact]
    public void ToRegionPointReturnsRelativePointWhenCursorIsInsideRegion()
    {
        var point = CursorCaptureCalculator.ToRegionPoint(new Point(150, 125), new CaptureRegion(100, 100, 640, 360));

        Assert.Equal(new Point(50, 25), point);
    }

    [Theory]
    [InlineData(99, 100)]
    [InlineData(100, 99)]
    [InlineData(740, 100)]
    [InlineData(100, 460)]
    public void ToRegionPointReturnsNullWhenCursorIsOutsideRegion(double x, double y)
    {
        var point = CursorCaptureCalculator.ToRegionPoint(new Point(x, y), new CaptureRegion(100, 100, 640, 360));

        Assert.Null(point);
    }
}
