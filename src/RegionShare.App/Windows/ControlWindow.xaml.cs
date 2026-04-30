using System.ComponentModel;
using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Overlay;
using RegionShare.App.Preview;

namespace RegionShare.App.Windows;

public partial class ControlWindow : Window
{
    private readonly IScreenCaptureService _captureService;
    private readonly PreviewCaptureController _captureController;
    private readonly IOverlayController _overlayController;
    private readonly IPreviewWindowController _previewWindowController;

    public ControlWindow(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider, IOverlayController overlayController, IPreviewWindowController previewWindowController)
    {
        _captureService = captureService;
        _captureController = new PreviewCaptureController(captureService, regionProvider);
        _overlayController = overlayController;
        _previewWindowController = previewWindowController;
        _overlayController.OverlayStateChanged += OverlayController_OverlayStateChanged;
        _previewWindowController.PreviewModeChanged += PreviewWindowController_PreviewModeChanged;

        InitializeComponent();
        UpdateControlState();
        UpdateOverlayState();
    }

    private void CaptureToggleButton_Click(object sender, RoutedEventArgs e)
    {
        _captureController.Toggle();
        UpdateControlState();
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

    private void PreviewModeToggleButton_Click(object sender, RoutedEventArgs e)
    {
        var nextMode = _previewWindowController.Mode == PreviewWindowMode.Borderless
            ? PreviewWindowMode.Normal
            : PreviewWindowMode.Borderless;

        _previewWindowController.SetMode(nextMode);
        UpdateControlState();
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _overlayController.OverlayStateChanged -= OverlayController_OverlayStateChanged;
        _previewWindowController.PreviewModeChanged -= PreviewWindowController_PreviewModeChanged;

        if (!Application.Current.Dispatcher.HasShutdownStarted)
        {
            Application.Current.Shutdown();
        }

        base.OnClosing(e);
    }

    private void OverlayController_OverlayStateChanged(object? sender, EventArgs e)
    {
        UpdateOverlayState();
    }

    private void PreviewWindowController_PreviewModeChanged(object? sender, EventArgs e)
    {
        UpdateControlState();
    }

    private void UpdateControlState()
    {
        var state = ControlWindowState.FromState(_captureService.IsCapturing, _previewWindowController.Mode);
        CaptureToggleButton.Content = state.CaptureToggleText;
        CaptureStatusText.Text = state.CaptureStatusText;
        PreviewModeToggleButton.Content = state.BorderlessToggleText;
    }

    private void UpdateOverlayState()
    {
        var controlState = PreviewOverlayControlState.FromOverlayState(_overlayController.IsLocked, _overlayController.IsOverlayVisible);

        OverlayLockToggleButton.Content = controlState.LockToggleText;
        OverlayVisibilityToggleButton.Content = controlState.VisibilityToggleText;
        OverlayStatusText.Text = controlState.StatusText;
    }
}
