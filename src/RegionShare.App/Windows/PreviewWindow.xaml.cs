using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Preview;

namespace RegionShare.App.Windows;

public partial class PreviewWindow : Window
{
    private readonly IScreenCaptureService _captureService;
    private readonly IPreviewWindowController _previewWindowController;
    private readonly Func<CaptureRegion> _regionProvider;

    public PreviewWindow(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider, IPreviewWindowController previewWindowController)
    {
        _captureService = captureService;
        _previewWindowController = previewWindowController;
        _regionProvider = regionProvider;
        captureService.FrameCaptured += CaptureService_FrameCaptured;
        previewWindowController.PreviewModeChanged += PreviewWindowController_PreviewModeChanged;
        InitializeComponent();
        UpdatePreviewMode();
        UpdatePreviewLayout();
    }

    private void PreviewViewport_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdatePreviewLayout();
    }

    protected override void OnClosed(EventArgs e)
    {
        _captureService.FrameCaptured -= CaptureService_FrameCaptured;
        _previewWindowController.PreviewModeChanged -= PreviewWindowController_PreviewModeChanged;
        if (_captureService is IDisposable disposableCaptureService)
        {
            disposableCaptureService.Dispose();
        }

        Application.Current.Shutdown();
        base.OnClosed(e);
    }

    private void CaptureService_FrameCaptured(object? sender, CapturedFrameEventArgs e)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => CaptureService_FrameCaptured(sender, e));
            return;
        }

        PreviewImage.Source = e.Frame;
        CapturePlaceholderText.Visibility = PreviewPlaceholderState.GetPlaceholderVisibility(true);
    }

    private void PreviewWindowController_PreviewModeChanged(object? sender, EventArgs e)
    {
        UpdatePreviewMode();
    }

    private void UpdatePreviewMode()
    {
        var state = PreviewWindowModeState.FromMode(_previewWindowController.Mode);
        WindowStyle = state.WindowStyle;
        ResizeMode = state.ResizeMode;
        PreviewViewport.Margin = state.ContentMargin;
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
