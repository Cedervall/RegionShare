namespace RegionShare.App.Capture;

public static class CaptureBackendLabelFormatter
{
    public static string Format(ScreenCaptureBackend? backend)
    {
        return backend switch
        {
            ScreenCaptureBackend.Direct3DDesktopDuplication => "GPU powered",
            ScreenCaptureBackend.Gdi => "Using CPU fallback",
            _ => "Capture stopped"
        };
    }
}
