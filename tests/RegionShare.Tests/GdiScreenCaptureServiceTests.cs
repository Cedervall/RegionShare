using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class GdiScreenCaptureServiceTests
{
    [Fact]
    public void StartRejectsInvalidRegion()
    {
        using var service = new GdiScreenCaptureService();

        Assert.Throws<ArgumentOutOfRangeException>(() => service.Start(new CaptureRegion(0, 0, 0, 720)));
        Assert.False(service.IsCapturing);
    }

    [Fact]
    public void StartSetsCaptureState()
    {
        using var service = new GdiScreenCaptureService();

        service.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.True(service.IsCapturing);
    }

    [Fact]
    public void StopClearsCaptureState()
    {
        using var service = new GdiScreenCaptureService();

        service.Start(new CaptureRegion(0, 0, 320, 180));
        service.Stop();

        Assert.False(service.IsCapturing);
    }

    [Fact]
    public void DisposeClearsCaptureState()
    {
        var service = new GdiScreenCaptureService();

        service.Start(new CaptureRegion(0, 0, 320, 180));
        service.Dispose();

        Assert.False(service.IsCapturing);
    }

    [Fact]
    public void StopIsSafeWhenCaptureIsAlreadyStopped()
    {
        using var service = new GdiScreenCaptureService();

        service.Stop();

        Assert.False(service.IsCapturing);
    }

    [Fact]
    public void StartThrowsAfterDispose()
    {
        var service = new GdiScreenCaptureService();

        service.Dispose();

        Assert.Throws<ObjectDisposedException>(() => service.Start(new CaptureRegion(0, 0, 320, 180)));
    }

    [Fact]
    public void RepeatedStartStopKeepsCaptureStateConsistent()
    {
        using var service = new GdiScreenCaptureService();

        service.Start(new CaptureRegion(0, 0, 320, 180));
        service.Stop();
        service.Start(new CaptureRegion(10, 10, 320, 180));
        service.Stop();

        Assert.False(service.IsCapturing);
    }
}
