namespace RegionShare.App.Capture;

public interface ICaptureBackendStatus
{
    event EventHandler? BackendChanged;

    ScreenCaptureBackend? CurrentBackend { get; }
}
