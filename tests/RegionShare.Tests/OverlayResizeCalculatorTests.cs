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

    [Fact]
    public void ResizeRightEdgeCanConstrainToSixteenByNine()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Right, 160, 0, MinimumSize, AspectRatioMode.SixteenByNine);

        Assert.Equal(100, resized.Left);
        Assert.Equal(100, resized.Top);
        Assert.Equal(800, resized.Width);
        Assert.Equal(450, resized.Height);
    }

    [Fact]
    public void ResizeBottomEdgeCanConstrainToFourByThree()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.Bottom, 0, 120, MinimumSize, AspectRatioMode.FourByThree);

        Assert.Equal(100, resized.Left);
        Assert.Equal(100, resized.Top);
        Assert.Equal(640, resized.Height * (4.0 / 3.0));
        Assert.Equal(480, resized.Height);
    }

    [Fact]
    public void ResizeCornerUsesHorizontalChangeWhenItIsDominantForAspectRatio()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.BottomRight, 160, 0, MinimumSize, AspectRatioMode.SixteenByNine);

        Assert.Equal(800, resized.Width);
        Assert.Equal(450, resized.Height);
    }

    [Fact]
    public void ResizeCornerUsesVerticalChangeWhenItIsDominantForAspectRatio()
    {
        var resized = OverlayResizeCalculator.Resize(InitialBounds, ResizeHandle.BottomRight, 0, 120, MinimumSize, AspectRatioMode.SixteenByNine);

        Assert.Equal(853.333333, resized.Width, 6);
        Assert.Equal(480, resized.Height);
    }
}
