namespace RegionShare.App.Capture;

public sealed class WindowsGraphicsCaptureService : IScreenCaptureService
{
    public bool IsCapturing { get; private set; }

    public void Start(CaptureRegion region)
    {
        if (!region.IsValid)
        {
            throw new ArgumentOutOfRangeException(nameof(region), "Capture region must have positive dimensions.");
        }

        IsCapturing = true;
    }

    public void Stop()
    {
        IsCapturing = false;
    }
}
