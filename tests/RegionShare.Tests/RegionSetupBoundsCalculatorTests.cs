using System.Windows;
using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class RegionSetupBoundsCalculatorTests
{
    [Fact]
    public void ApplyReturnsEmptyWhenOverlayIsLocked()
    {
        var bounds = RegionSetupBoundsCalculator.Apply(new Rect(10, 20, 640, 360), new Size(320, 180), true);

        Assert.True(bounds.IsEmpty);
    }

    [Fact]
    public void ApplyKeepsValidUnlockedBounds()
    {
        var requestedBounds = new Rect(10, 20, 640, 360);

        var bounds = RegionSetupBoundsCalculator.Apply(requestedBounds, new Size(320, 180), false);

        Assert.Equal(requestedBounds, bounds);
    }

    [Fact]
    public void ApplyClampsUnlockedBoundsToMinimumSize()
    {
        var bounds = RegionSetupBoundsCalculator.Apply(new Rect(10, 20, 100, 50), new Size(320, 180), false);

        Assert.Equal(new Rect(10, 20, 320, 180), bounds);
    }
}
