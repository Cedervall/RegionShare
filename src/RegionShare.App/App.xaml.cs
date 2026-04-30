using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Dpi;
using RegionShare.App.Overlay;
using RegionShare.App.Windows;

namespace RegionShare.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var overlayState = new OverlayStateService();
        var captureService = new GdiScreenCaptureService();
        var overlayWindow = new OverlayWindow(overlayState, new DpiService(), new WindowCaptureExclusionService());
        var previewWindow = new PreviewWindow(captureService, overlayWindow.GetCaptureRegion);

        previewWindow.Show();
        overlayWindow.Show();
    }
}

