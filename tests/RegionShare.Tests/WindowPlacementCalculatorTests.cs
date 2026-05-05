using System.Windows;
using RegionShare.App.Windowing;

namespace RegionShare.Tests;

public sealed class WindowPlacementCalculatorTests
{
    [Fact]
    public void EnsureVisibleKeepsBoundsWhenTheyIntersectAWorkArea()
    {
        var bounds = WindowPlacementCalculator.EnsureVisible(
            new Rect(100, 100, 640, 360),
            new Size(320, 180),
            [new Rect(0, 0, 1920, 1080)],
            new Point(24, 24));

        Assert.Equal(new Rect(100, 100, 640, 360), bounds);
    }

    [Fact]
    public void EnsureVisibleResetsOffscreenBoundsToFallbackOffset()
    {
        var bounds = WindowPlacementCalculator.EnsureVisible(
            new Rect(2500, 100, 640, 360),
            new Size(320, 180),
            [new Rect(0, 0, 1920, 1080)],
            new Point(80, 80));

        Assert.Equal(new Rect(80, 80, 640, 360), bounds);
    }

    [Fact]
    public void EnsureVisibleAllowsNegativeCoordinatesOnActiveMonitor()
    {
        var bounds = WindowPlacementCalculator.EnsureVisible(
            new Rect(-1000, 100, 640, 360),
            new Size(320, 180),
            [new Rect(-1920, 0, 1920, 1080), new Rect(0, 0, 1920, 1080)],
            new Point(24, 24));

        Assert.Equal(new Rect(-1000, 100, 640, 360), bounds);
    }

    [Fact]
    public void EnsureVisibleClampsOversizedBoundsToWorkArea()
    {
        var bounds = WindowPlacementCalculator.EnsureVisible(
            new Rect(10, 10, 3000, 2000),
            new Size(320, 180),
            [new Rect(0, 0, 1920, 1040)],
            new Point(24, 24));

        Assert.Equal(new Rect(0, 0, 1920, 1040), bounds);
    }

    [Fact]
    public void EnsureVisibleFallsBackToNormalizedBoundsWithoutWorkAreas()
    {
        var bounds = WindowPlacementCalculator.EnsureVisible(
            new Rect(double.NaN, double.PositiveInfinity, 0, 0),
            new Size(320, 180),
            [],
            new Point(24, 24));

        Assert.Equal(new Rect(0, 0, 320, 180), bounds);
    }
}
