using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewOverlayControlStateTests
{
    [Theory]
    [InlineData(false, true, "Lock Overlay", "Hide Overlay", "Overlay: Unlocked & Visible")]
    [InlineData(true, true, "Unlock Overlay", "Hide Overlay", "Overlay: Locked & Visible")]
    [InlineData(false, false, "Lock Overlay", "Show Overlay", "Overlay: Unlocked & Hidden")]
    [InlineData(true, false, "Unlock Overlay", "Show Overlay", "Overlay: Locked & Hidden")]
    public void FromOverlayStateReturnsPreviewControlLabels(bool isLocked, bool isVisible, string lockText, string visibilityText, string statusText)
    {
        var state = PreviewOverlayControlState.FromOverlayState(isLocked, isVisible);

        Assert.Equal(lockText, state.LockToggleText);
        Assert.Equal(visibilityText, state.VisibilityToggleText);
        Assert.Equal(statusText, state.StatusText);
    }
}
