namespace RegionShare.App.Capture;

public interface IScreenCaptureService
{
    bool IsCapturing { get; }

    void Start(CaptureRegion region);

    void Stop();
}
