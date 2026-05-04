using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class ScreenCaptureBackendSelectorTests
{
    [Fact]
    public void SelectUsesDirect3DWhenSupportedAndCursorCaptureIsDisabled()
    {
        var backend = ScreenCaptureBackendSelector.Select(false, true);

        Assert.Equal(ScreenCaptureBackend.Direct3DDesktopDuplication, backend);
    }

    [Fact]
    public void SelectUsesGdiWhenDirect3DIsUnsupported()
    {
        var backend = ScreenCaptureBackendSelector.Select(false, false);

        Assert.Equal(ScreenCaptureBackend.Gdi, backend);
    }

    [Fact]
    public void SelectUsesDirect3DWhenSupportedAndCursorCaptureIsEnabled()
    {
        var backend = ScreenCaptureBackendSelector.Select(true, true);

        Assert.Equal(ScreenCaptureBackend.Direct3DDesktopDuplication, backend);
    }
}
