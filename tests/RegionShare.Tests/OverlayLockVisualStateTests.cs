using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class OverlayLockVisualStateTests
{
    [Fact]
    public void FromLockStateReturnsUnlockedVisualState()
    {
        var state = OverlayLockVisualState.FromLockState(false);

        Assert.Equal("#22C55E", state.BorderBrush);
        Assert.Equal("Unlocked", state.StatusText);
        Assert.Equal("Lock", state.ToggleText);
        Assert.Equal("Unlocked region size", state.SizeToolTip);
    }

    [Fact]
    public void FromLockStateReturnsLockedVisualState()
    {
        var state = OverlayLockVisualState.FromLockState(true);

        Assert.Equal("#F59E0B", state.BorderBrush);
        Assert.Equal("Locked", state.StatusText);
        Assert.Equal("Unlock", state.ToggleText);
        Assert.Equal("Locked region size", state.SizeToolTip);
    }
}
