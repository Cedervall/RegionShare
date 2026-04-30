using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewOverlayControlStateTests
{
    [Theory]
    [InlineData(false, true, "Lock overlay", "Hide overlay", "Overlay unlocked, visible")]
    [InlineData(true, true, "Unlock overlay", "Hide overlay", "Overlay locked, visible")]
    [InlineData(false, false, "Lock overlay", "Show overlay", "Overlay unlocked, hidden")]
    [InlineData(true, false, "Unlock overlay", "Show overlay", "Overlay locked, hidden")]
    public void FromOverlayStateReturnsPreviewControlLabels(bool isLocked, bool isVisible, string lockText, string visibilityText, string statusText)
    {
        var state = PreviewOverlayControlState.FromOverlayState(isLocked, isVisible);

        Assert.Equal(lockText, state.LockToggleText);
        Assert.Equal(visibilityText, state.VisibilityToggleText);
        Assert.Equal(statusText, state.StatusText);
    }
}
