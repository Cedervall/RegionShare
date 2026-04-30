using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class OverlayInteractionGuardTests
{
    [Fact]
    public void CanMoveAndResizeWhenOverlayIsUnlocked()
    {
        var overlayState = new OverlayStateService();

        Assert.True(OverlayInteractionGuard.CanMove(overlayState));
        Assert.True(OverlayInteractionGuard.CanResize(overlayState));
    }

    [Fact]
    public void CannotMoveOrResizeWhenOverlayIsLocked()
    {
        var overlayState = new OverlayStateService();

        overlayState.ToggleLock();

        Assert.False(OverlayInteractionGuard.CanMove(overlayState));
        Assert.False(OverlayInteractionGuard.CanResize(overlayState));
    }
}
