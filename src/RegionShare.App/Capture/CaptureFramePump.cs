using System.Windows.Media.Imaging;
using System.Runtime.InteropServices;

namespace RegionShare.App.Capture;

public static class CaptureFramePump
{
    public static void CaptureNextFrame(
        bool isCapturing,
        CaptureRegion? region,
        Func<CaptureRegion, BitmapSource> captureFrame,
        Action<BitmapSource> publishFrame,
        Action stopCapture,
        Action<Exception> publishFailure)
    {
        if (!isCapturing || region is null)
        {
            return;
        }

        try
        {
            publishFrame(captureFrame(region));
        }
        catch (Exception exception) when (exception is InvalidOperationException or ExternalException)
        {
            stopCapture();
            publishFailure(exception);
        }
    }
}
