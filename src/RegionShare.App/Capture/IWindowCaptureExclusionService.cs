using System.Windows;

namespace RegionShare.App.Capture;

public interface IWindowCaptureExclusionService
{
    bool ExcludeFromCapture(Window window);
}
