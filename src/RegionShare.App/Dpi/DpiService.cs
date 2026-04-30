using System.Windows;
using RegionShare.App.Capture;

namespace RegionShare.App.Dpi;

public sealed class DpiService : IDpiService
{
    public CaptureRegion ToPhysicalRegion(Rect logicalRegion, double dpiScaleX, double dpiScaleY)
    {
        return new CaptureRegion(
            (int)Math.Round(logicalRegion.X * dpiScaleX),
            (int)Math.Round(logicalRegion.Y * dpiScaleY),
            (int)Math.Round(logicalRegion.Width * dpiScaleX),
            (int)Math.Round(logicalRegion.Height * dpiScaleY));
    }
}
