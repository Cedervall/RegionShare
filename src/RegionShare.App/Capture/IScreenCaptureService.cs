namespace RegionShare.App.Capture;

public interface IScreenCaptureService
{
    event EventHandler<CapturedFrameEventArgs>? FrameCaptured;

    bool IsCapturing { get; }

    void Start(CaptureRegion region);

    void Stop();
}
