namespace RegionShare.App.Capture;

public interface IScreenCaptureService
{
    event EventHandler<CapturedFrameEventArgs>? FrameCaptured;

    event EventHandler<CaptureFailedEventArgs>? CaptureFailed;

    bool IsCapturing { get; }

    void Start(CaptureRegion region);

    void Stop();
}
