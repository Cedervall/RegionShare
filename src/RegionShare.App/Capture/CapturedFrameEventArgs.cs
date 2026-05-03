using System.Windows.Media.Imaging;

namespace RegionShare.App.Capture;

public sealed class CapturedFrameEventArgs : EventArgs
{
    public CapturedFrameEventArgs(BitmapSource frame, long capturedTimestamp)
    {
        Frame = frame;
        CapturedTimestamp = capturedTimestamp;
    }

    public BitmapSource Frame { get; }

    public long CapturedTimestamp { get; }
}
