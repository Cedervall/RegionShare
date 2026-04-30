namespace RegionShare.App.Capture;

public interface IScreenCaptureBackendSupport
{
    bool IsDirect3DDesktopDuplicationSupported { get; }
}
