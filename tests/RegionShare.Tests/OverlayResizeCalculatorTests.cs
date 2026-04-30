using System.Windows;
using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class OverlayResizeCalculatorTests
{
    private static readonly Rect InitialBounds = new(100, 100, 640, 360);

    private static readonly Size MinimumSize = new(320, 180);

    [Fact]
    public void ResizeRightEdgeIncreasesWidth()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Right, 50, 0, MinimumSize);

        Assert.Equal(new Rect(100, 100, 690, 360), resized);
    }

    [Fact]
    public void ResizeBottomEdgeIncreasesHeight()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Bottom, 0, 40, MinimumSize);

        Assert.Equal(new Rect(100, 100, 640, 400), resized);
    }

    [Fact]
    public void ResizeLeftEdgeChangesLeftAndWidth()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Left, 60, 0, MinimumSize);

        Assert.Equal(new Rect(160, 100, 580, 360), resized);
    }

    [Fact]
    public void ResizeTopEdgeChangesTopAndHeight()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Top, 0, 70, MinimumSize);

        Assert.Equal(new Rect(100, 170, 640, 290), resized);
    }

    [Fact]
    public void ResizeCornerChangesBothAxes()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.BottomRight, 25, 35, MinimumSize);

        Assert.Equal(new Rect(100, 100, 665, 395), resized);
    }

    [Fact]
    public void ResizeRightEdgeDoesNotShrinkBelowMinimumWidth()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Right, -400, 0, MinimumSize);

        Assert.Equal(new Rect(100, 100, 320, 360), resized);
    }

    [Fact]
    public void ResizeLeftEdgePastMinimumKeepsRightEdgeFixed()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Left, 500, 0, MinimumSize);

        Assert.Equal(new Rect(420, 100, 320, 360), resized);
    }

    [Fact]
    public void ResizeTopEdgePastMinimumKeepsBottomEdgeFixed()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Top, 0, 300, MinimumSize);

        Assert.Equal(new Rect(100, 280, 640, 180), resized);
    }
}
