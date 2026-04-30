namespace RegionShare.App.Capture;

public static class ScreenCaptureServiceFactory
{
    public static IScreenCaptureService Create(ICursorCaptureSettings cursorCaptureSettings, ICaptureFrameRateSettings captureFrameRateSettings, IScreenCaptureBackendSupport backendSupport)
    {
        return ScreenCaptureBackendSelector.Select(cursorCaptureSettings.IsCursorCaptureEnabled, backendSupport.IsDirect3DDesktopDuplicationSupported) switch
        {
            ScreenCaptureBackend.Direct3DDesktopDuplication => new Direct3DDesktopDuplicationScreenCaptureService(captureFrameRateSettings),
            _ => new GdiScreenCaptureService(cursorCaptureSettings, captureFrameRateSettings)
        };
    }
}
