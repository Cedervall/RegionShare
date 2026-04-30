using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using RegionShare.App.Capture;
using RegionShare.App.Hotkeys;
using RegionShare.App.Overlay;
using RegionShare.App.Preview;

namespace RegionShare.App.Windows;

public partial class ControlWindow : Window
{
    private readonly IScreenCaptureService _captureService;
    private readonly PreviewCaptureController _captureController;
    private readonly IOverlayController _overlayController;
    private readonly IPreviewWindowController _previewWindowController;
    private readonly IGlobalHotkeyService _hotkeyService;
    private readonly ICursorCaptureSettings _cursorCaptureSettings;

    public ControlWindow(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider, IOverlayController overlayController, IPreviewWindowController previewWindowController, IGlobalHotkeyService hotkeyService, ICursorCaptureSettings cursorCaptureSettings)
    {
        _captureService = captureService;
        _captureController = new PreviewCaptureController(captureService, regionProvider);
        _overlayController = overlayController;
        _previewWindowController = previewWindowController;
        _hotkeyService = hotkeyService;
        _cursorCaptureSettings = cursorCaptureSettings;
        _captureService.CaptureFailed += CaptureService_CaptureFailed;
        _overlayController.OverlayStateChanged += OverlayController_OverlayStateChanged;
        _previewWindowController.PreviewModeChanged += PreviewWindowController_PreviewModeChanged;

        InitializeComponent();
        AspectRatioComboBox.ItemsSource = Enum.GetValues<AspectRatioMode>();
        AspectRatioComboBox.SelectedItem = _overlayController.AspectRatioMode;
        UpdateCursorCaptureState();
        UpdateControlState();
        UpdateOverlayState();
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        _hotkeyService.RegisterLockToggle(() => Dispatcher.Invoke(() =>
        {
            _overlayController.ToggleLock();
            UpdateOverlayState();
        }));
        _hotkeyService.RegisterOverlayVisibilityToggle(() => Dispatcher.Invoke(() =>
        {
            _overlayController.ToggleOverlayVisibility();
            UpdateOverlayState();
        }));
        _hotkeyService.RegisterWindow(this);
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

    private void PresetButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { Tag: string tag })
        {
            return;
        }

        var preset = tag switch
        {
            "1280x720" => PresetSize.Hd,
            "1600x900" => PresetSize.HdPlus,
            "1920x1080" => PresetSize.FullHd,
            _ => null
        };

        if (preset is null)
        {
            return;
        }

        _overlayController.ApplyPreset(preset);
    }

    private void AspectRatioComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (AspectRatioComboBox.SelectedItem is AspectRatioMode aspectRatioMode)
        {
            _overlayController.SetAspectRatioMode(aspectRatioMode);
        }
    }

    private void CursorCaptureCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        var state = CursorCaptureToggleController.Apply(CursorCaptureCheckBox.IsChecked == true, _cursorCaptureSettings);
        ApplyCursorCaptureState(state);
    }

    private void ExitButton_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        _overlayController.OverlayStateChanged -= OverlayController_OverlayStateChanged;
        _previewWindowController.PreviewModeChanged -= PreviewWindowController_PreviewModeChanged;
        _hotkeyService.UnregisterAll();
        _captureService.CaptureFailed -= CaptureService_CaptureFailed;

        if (!Application.Current.Dispatcher.HasShutdownStarted)
        {
            Application.Current.Shutdown();
        }

        base.OnClosing(e);
    }

    private void OverlayController_OverlayStateChanged(object? sender, EventArgs e)
    {
        UpdateOverlayState();
        AspectRatioComboBox.SelectedItem = _overlayController.AspectRatioMode;
    }

    private void PreviewWindowController_PreviewModeChanged(object? sender, EventArgs e)
    {
        UpdateControlState();
    }

    private void CaptureService_CaptureFailed(object? sender, CaptureFailedEventArgs e)
    {
        CaptureErrorText.Text = "Capture stopped: " + e.Exception.Message;
        UpdateControlState();
    }

    private void UpdateControlState()
    {
        var state = ControlWindowState.FromState(_captureService.IsCapturing, _previewWindowController.Mode);
        CaptureToggleButton.Content = state.CaptureToggleText;
        CaptureStatusText.Text = state.CaptureStatusText;
        PreviewModeToggleButton.Content = state.BorderlessToggleText;
        if (_captureService.IsCapturing)
        {
            CaptureErrorText.Text = string.Empty;
        }
    }

    private void UpdateOverlayState()
    {
        var controlState = PreviewOverlayControlState.FromOverlayState(_overlayController.IsLocked, _overlayController.IsOverlayVisible);

        OverlayLockToggleButton.Content = controlState.LockToggleText;
        OverlayVisibilityToggleButton.Content = controlState.VisibilityToggleText;
        OverlayStatusText.Text = controlState.StatusText;
    }

    private void UpdateCursorCaptureState()
    {
        var state = CursorCaptureControlState.FromEnabled(_cursorCaptureSettings.IsCursorCaptureEnabled);
        ApplyCursorCaptureState(state);
    }

    private void ApplyCursorCaptureState(CursorCaptureControlState state)
    {
        CursorCaptureCheckBox.IsChecked = state.IsChecked;
        CursorCaptureCheckBox.Content = state.Label;
    }
}
