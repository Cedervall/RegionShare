using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Preview;

namespace RegionShare.App.Windows;

public partial class PreviewWindow : Window
{
    private readonly PreviewCaptureController _captureController;
    private readonly Func<CaptureRegion> _regionProvider;

    public PreviewWindow(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider)
    {
        _regionProvider = regionProvider;
        _captureController = new PreviewCaptureController(captureService, regionProvider);
        InitializeComponent();
        UpdateCaptureState();
        UpdatePreviewLayout();
    }

    private void CaptureToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _captureController.Toggle();
        UpdateCaptureState();
    }

    private void PreviewViewport_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdatePreviewLayout();
    }

    protected override void OnClosed(EventArgs e)
    {
        _captureController.Stop();
        base.OnClosed(e);
    }

    private void UpdateCaptureState()
    {
        CaptureToggleButton.Content = _captureController.IsCapturing ? "Stop capture" : "Start capture";
        CaptureStatusText.Text = _captureController.IsCapturing ? "Capturing" : "Stopped";
    }

    private void UpdatePreviewLayout()
    {
        if (PreviewViewport is null || PreviewPlaceholder is null)
        {
            return;
        }

        var region = _regionProvider();
        var fittedSize = PreviewFitCalculator.Fit(
            new Size(region.Width, region.Height),
            new Size(PreviewViewport.ActualWidth, PreviewViewport.ActualHeight));

        if (fittedSize.IsEmpty)
        {
            PreviewPlaceholder.Width = double.NaN;
            PreviewPlaceholder.Height = double.NaN;
            return;
        }

        PreviewPlaceholder.Width = fittedSize.Width;
        PreviewPlaceholder.Height = fittedSize.Height;
    }
}
