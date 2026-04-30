using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Dpi;
using RegionShare.App.Hotkeys;
using RegionShare.App.Overlay;
using RegionShare.App.Preview;
using RegionShare.App.Windowing;
using RegionShare.App.Windows;

namespace RegionShare.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var overlayState = new OverlayStateService();
        var captureService = new GdiScreenCaptureService();
        var hotkeyService = new GlobalHotkeyService();
        var previewWindowController = new PreviewWindowController();
        var overlayWindow = new OverlayWindow(overlayState, new DpiService(), new WindowCaptureExclusionService(), new WindowClickThroughService());
        var previewWindow = new PreviewWindow(captureService, overlayWindow.GetCaptureRegion, previewWindowController);
        var controlWindow = new ControlWindow(captureService, overlayWindow.GetCaptureRegion, overlayWindow, previewWindowController, hotkeyService);

        overlayWindow.Show();
        previewWindow.Show();
        controlWindow.Show();
    }
}

