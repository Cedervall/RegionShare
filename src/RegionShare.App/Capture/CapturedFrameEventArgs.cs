using System.Windows.Media.Imaging;

namespace RegionShare.App.Capture;

public sealed class CapturedFrameEventArgs : EventArgs
{
    public CapturedFrameEventArgs(BitmapSource frame)
    {
        Frame = frame;
    }

    public BitmapSource Frame { get; }
}
