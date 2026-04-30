using RegionShare.App.Hotkeys;

namespace RegionShare.Tests;

public sealed class GlobalHotkeyServiceTests
{
    [Fact]
    public void TryInvokeRunsRegisteredLockToggleCallback()
    {
        var service = new GlobalHotkeyService();
        var wasCalled = false;

        service.RegisterLockToggle(() => wasCalled = true);

        Assert.True(service.TryInvoke(GlobalHotkeyAction.ToggleLock));
        Assert.True(wasCalled);
    }

    [Fact]
    public void TryInvokeRunsRegisteredOverlayVisibilityCallback()
    {
        var service = new GlobalHotkeyService();
        var wasCalled = false;

        service.RegisterOverlayVisibilityToggle(() => wasCalled = true);

        Assert.True(service.TryInvoke(GlobalHotkeyAction.ToggleOverlayVisibility));
        Assert.True(wasCalled);
    }

    [Fact]
    public void TryInvokeReturnsFalseWhenCallbackIsNotRegistered()
    {
        var service = new GlobalHotkeyService();

        Assert.False(service.TryInvoke(GlobalHotkeyAction.ToggleLock));
    }

    [Fact]
    public void UnregisterAllRemovesCallbacks()
    {
        var service = new GlobalHotkeyService();
        var count = 0;
        service.RegisterLockToggle(() => count++);

        service.UnregisterAll();

        Assert.False(service.TryInvoke(GlobalHotkeyAction.ToggleLock));
        Assert.Equal(0, count);
    }
}
