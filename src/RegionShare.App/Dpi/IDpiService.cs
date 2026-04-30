using System.Windows;
using RegionShare.App.Capture;

namespace RegionShare.App.Dpi;

public interface IDpiService
{
    CaptureRegion ToPhysicalRegion(Rect logicalRegion, double dpiScaleX, double dpiScaleY);
}
