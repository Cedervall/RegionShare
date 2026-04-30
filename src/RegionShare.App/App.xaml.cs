using System.Windows;
using RegionShare.App.Overlay;
using RegionShare.App.Windows;

namespace RegionShare.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var overlayState = new OverlayStateService();
        var previewWindow = new PreviewWindow();
        var overlayWindow = new OverlayWindow(overlayState);

        previewWindow.Show();
        overlayWindow.Show();
    }
}

