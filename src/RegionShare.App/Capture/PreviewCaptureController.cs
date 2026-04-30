namespace RegionShare.App.Capture;

public sealed class PreviewCaptureController
{
    private readonly IScreenCaptureService _captureService;
    private readonly Func<CaptureRegion> _regionProvider;

    public PreviewCaptureController(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider)
    {
        _captureService = captureService;
        _regionProvider = regionProvider;
    }

    public bool IsCapturing => _captureService.IsCapturing;

    public void Toggle()
    {
        if (_captureService.IsCapturing)
        {
            Stop();
            return;
        }

        _captureService.Start(_regionProvider());
    }

    public void Stop()
    {
        if (!_captureService.IsCapturing)
        {
            return;
        }

        _captureService.Stop();
    }
}
