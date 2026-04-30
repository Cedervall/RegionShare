using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class PreviewCaptureControllerTests
{
    [Fact]
    public void ToggleStartsCaptureWithCurrentRegionWhenStopped()
    {
        var captureService = new FakeScreenCaptureService();
        var region = new CaptureRegion(10, 20, 640, 360);
        var controller = new PreviewCaptureController(captureService, () => region);

        controller.Toggle();

        Assert.True(controller.IsCapturing);
        Assert.Equal(region, captureService.StartedRegion);
        Assert.Equal(1, captureService.StartCount);
        Assert.Equal(0, captureService.StopCount);
    }

    [Fact]
    public void ToggleStopsCaptureWhenAlreadyCapturing()
    {
        var captureService = new FakeScreenCaptureService();
        var controller = new PreviewCaptureController(captureService, () => new CaptureRegion(10, 20, 640, 360));

        controller.Toggle();
        controller.Toggle();

        Assert.False(controller.IsCapturing);
        Assert.Equal(1, captureService.StartCount);
        Assert.Equal(1, captureService.StopCount);
    }

    [Fact]
    public void StopStopsActiveCapture()
    {
        var captureService = new FakeScreenCaptureService();
        var controller = new PreviewCaptureController(captureService, () => new CaptureRegion(10, 20, 640, 360));

        controller.Toggle();
        controller.Stop();

        Assert.False(controller.IsCapturing);
        Assert.Equal(1, captureService.StopCount);
    }

    [Fact]
    public void StopDoesNothingWhenCaptureIsAlreadyStopped()
    {
        var captureService = new FakeScreenCaptureService();
        var controller = new PreviewCaptureController(captureService, () => new CaptureRegion(10, 20, 640, 360));

        controller.Stop();

        Assert.False(controller.IsCapturing);
        Assert.Equal(0, captureService.StopCount);
    }

    private sealed class FakeScreenCaptureService : IScreenCaptureService
    {
        public bool IsCapturing { get; private set; }

        public CaptureRegion? StartedRegion { get; private set; }

        public int StartCount { get; private set; }

        public int StopCount { get; private set; }

        public void Start(CaptureRegion region)
        {
            StartedRegion = region;
            StartCount++;
            IsCapturing = true;
        }

        public void Stop()
        {
            StopCount++;
            IsCapturing = false;
        }
    }
}
