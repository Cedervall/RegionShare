using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class OverlayStateServiceTests
{
    [Fact]
    public void ToggleLockFlipsLockState()
    {
        var service = new OverlayStateService();

        service.ToggleLock();

        Assert.True(service.IsLocked);

        service.ToggleLock();

        Assert.False(service.IsLocked);
    }
}
