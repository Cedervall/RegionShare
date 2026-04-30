using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class CaptureFramePumpTests
{
    [Fact]
    public void CaptureNextFrameDoesNothingWhenCaptureIsStopped()
    {
        var captureWasCalled = false;

        CaptureFramePump.CaptureNextFrame(
            false,
            new CaptureRegion(0, 0, 320, 180),
            _ =>
            {
                captureWasCalled = true;
                throw new InvalidOperationException();
            },
            _ => { },
            () => { },
            _ => { });

        Assert.False(captureWasCalled);
    }

    [Fact]
    public void CaptureNextFrameStopsCaptureAndPublishesFailureWhenCaptureFails()
    {
        var stopWasCalled = false;
        Exception? publishedException = null;
        var expectedException = new InvalidOperationException("capture failed");

        CaptureFramePump.CaptureNextFrame(
            true,
            new CaptureRegion(0, 0, 320, 180),
            _ => throw expectedException,
            _ => { },
            () => stopWasCalled = true,
            exception => publishedException = exception);

        Assert.True(stopWasCalled);
        Assert.Same(expectedException, publishedException);
    }

    [Fact]
    public void CaptureNextFrameStopsCaptureAndPublishesFailureForExternalCaptureFailure()
    {
        var stopWasCalled = false;
        Exception? publishedException = null;
        var expectedException = new ExternalException("gdi failed");

        CaptureFramePump.CaptureNextFrame(
            true,
            new CaptureRegion(0, 0, 320, 180),
            _ => throw expectedException,
            _ => { },
            () => stopWasCalled = true,
            exception => publishedException = exception);

        Assert.True(stopWasCalled);
        Assert.Same(expectedException, publishedException);
    }
}
