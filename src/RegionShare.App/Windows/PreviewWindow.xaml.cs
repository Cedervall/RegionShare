using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Overlay;
using RegionShare.App.Preview;

namespace RegionShare.App.Windows;

public partial class PreviewWindow : Window
{
    private readonly IScreenCaptureService _captureService;
    private readonly PreviewCaptureController _captureController;
    private readonly IOverlayController _overlayController;
    private readonly Func<CaptureRegion> _regionProvider;

    public PreviewWindow(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider, IOverlayController overlayController)
    {
        _captureService = captureService;
        _overlayController = overlayController;
        _regionProvider = regionProvider;
        _captureController = new PreviewCaptureController(captureService, regionProvider);
        captureService.FrameCaptured += CaptureService_FrameCaptured;
        overlayController.OverlayStateChanged += OverlayController_StateChanged;
        InitializeComponent();
        UpdateCaptureState();
        UpdateOverlayState();
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

    private void OverlayLockToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _overlayController.ToggleLock();
        UpdateOverlayState();
    }

    private void OverlayVisibilityToggleButton_Click(object sender, RoutedEventArgs e)
    {
        if (_overlayController.IsOverlayVisible)
        {
            _overlayController.HideOverlay();
        }
        else
        {
            _overlayController.ShowOverlay();
        }

        UpdateOverlayState();
    }

    protected override void OnClosed(EventArgs e)
    {
        _captureController.Stop();
        _captureService.FrameCaptured -= CaptureService_FrameCaptured;
        _overlayController.OverlayStateChanged -= OverlayController_StateChanged;
        if (_captureService is IDisposable disposableCaptureService)
        {
            disposableCaptureService.Dispose();
        }

        base.OnClosed(e);
    }

    private void CaptureService_FrameCaptured(object? sender, CapturedFrameEventArgs e)
    {
        PreviewImage.Source = e.Frame;
        CapturePlaceholderText.Visibility = PreviewPlaceholderState.GetPlaceholderVisibility(true);
    }

    private void OverlayController_StateChanged(object? sender, EventArgs e)
    {
        UpdateOverlayState();
    }

    private void UpdateCaptureState()
    {
        CaptureToggleButton.Content = _captureController.IsCapturing ? "Stop capture" : "Start capture";
        CaptureStatusText.Text = _captureController.IsCapturing ? "Capturing" : "Stopped";
    }

    private void UpdateOverlayState()
    {
        var controlState = PreviewOverlayControlState.FromOverlayState(_overlayController.IsLocked, _overlayController.IsOverlayVisible);

        OverlayLockToggleButton.Content = controlState.LockToggleText;
        OverlayVisibilityToggleButton.Content = controlState.VisibilityToggleText;
        OverlayStatusText.Text = controlState.StatusText;
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
