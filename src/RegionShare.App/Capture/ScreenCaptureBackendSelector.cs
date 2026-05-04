namespace RegionShare.App.Capture;

public static class ScreenCaptureBackendSelector
{
    public static ScreenCaptureBackend Select(bool isCursorCaptureEnabled, bool isDirect3DDesktopDuplicationSupported)
    {
        if (!isDirect3DDesktopDuplicationSupported)
        {
            return ScreenCaptureBackend.Gdi;
        }

        return ScreenCaptureBackend.Direct3DDesktopDuplication;
    }
}
