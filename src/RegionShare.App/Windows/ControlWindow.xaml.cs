using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
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
    private readonly IPreviewBlackoutController _previewBlackoutController;
    private readonly IGlobalHotkeyService _hotkeyService;
    private readonly ICursorCaptureSettings _cursorCaptureSettings;
    private readonly ICaptureFrameRateSettings _captureFrameRateSettings;
    private RegionSetupWindow? _regionSetupWindow;
    private bool _restoreOverlayAfterRegionSetup;
    private bool _isApplyingPreset;
    private bool _isChangingAspectRatio;
    private string? _selectedPresetKey;
    private Rect _lastRegionBounds;

    public ControlWindow(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider, IOverlayController overlayController, IPreviewWindowController previewWindowController, IPreviewBlackoutController previewBlackoutController, IGlobalHotkeyService hotkeyService, ICursorCaptureSettings cursorCaptureSettings, ICaptureFrameRateSettings captureFrameRateSettings)
    {
        _captureService = captureService;
        _captureController = new PreviewCaptureController(captureService, regionProvider);
        _overlayController = overlayController;
        _previewWindowController = previewWindowController;
        _previewBlackoutController = previewBlackoutController;
        _hotkeyService = hotkeyService;
        _cursorCaptureSettings = cursorCaptureSettings;
        _captureFrameRateSettings = captureFrameRateSettings;
        _captureService.CaptureFailed += CaptureService_CaptureFailed;
        _overlayController.OverlayStateChanged += OverlayController_OverlayStateChanged;
        _previewWindowController.PreviewModeChanged += PreviewWindowController_PreviewModeChanged;

        InitializeComponent();
        CaptureFrameRateComboBox.ItemsSource = CaptureFrameRateCalculator.SupportedFramesPerSecond;
        CaptureFrameRateComboBox.SelectedItem = CaptureFrameRateCalculator.Sanitize(_captureFrameRateSettings.FramesPerSecond);
        UpdateCursorCaptureState();
        UpdateCaptureFrameRateState();
        UpdateControlState();
        UpdateOverlayState();
        UpdateOverlayDisplayOptionsState();
        UpdateAspectRatioState();
        UpdatePresetButtons();
        _lastRegionBounds = _overlayController.RegionBounds;
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
        var wasCapturing = _captureService.IsCapturing;
        _captureController.Toggle();
        if (wasCapturing && !_captureService.IsCapturing)
        {
            _previewBlackoutController.RequestBlackout();
        }

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

    private void PresetButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { Tag: string tag })
        {
            return;
        }

        var preset = PresetSize.FromKey(tag);

        if (preset is null)
        {
            return;
        }

        _selectedPresetKey = preset.Key;
        _isApplyingPreset = true;
        try
        {
            _overlayController.ApplyPreset(preset);
        }
        finally
        {
            _isApplyingPreset = false;
        }

        _lastRegionBounds = _overlayController.RegionBounds;
        UpdatePresetButtons();
    }

    private void CursorCaptureCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        var state = CursorCaptureToggleController.Apply(CursorCaptureCheckBox.IsChecked == true, _cursorCaptureSettings);
        ApplyCursorCaptureState(state);
    }

    private void BorderlessPreviewCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        var nextMode = BorderlessPreviewCheckBox.IsChecked == true
            ? PreviewWindowMode.Borderless
            : PreviewWindowMode.Normal;

        _previewWindowController.SetMode(nextMode);
        UpdateControlState();
    }

    private void OverlayStatusCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        _overlayController.SetStatusVisibility(OverlayStatusCheckBox.IsChecked == true);
        UpdateOverlayDisplayOptionsState();
    }

    private void OverlayLatencyCheckBox_Changed(object sender, RoutedEventArgs e)
    {
        _overlayController.SetLatencyVisibility(OverlayLatencyCheckBox.IsChecked == true);
        UpdateOverlayDisplayOptionsState();
    }

    private void CaptureFrameRateComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (CaptureFrameRateComboBox.SelectedItem is int framesPerSecond)
        {
            var state = CaptureFrameRateToggleController.Apply(framesPerSecond, _captureFrameRateSettings);
            ApplyCaptureFrameRateState(state);
        }
    }

    private void AspectRatioButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not FrameworkElement { Tag: string tag } || !Enum.TryParse<AspectRatioMode>(tag, out var aspectRatioMode))
        {
            return;
        }

        _selectedPresetKey = null;
        _isChangingAspectRatio = true;
        try
        {
            _overlayController.SetAspectRatioMode(aspectRatioMode);
        }
        finally
        {
            _isChangingAspectRatio = false;
        }

        UpdateAspectRatioState();
        UpdatePresetButtons();
    }

    private void FancyZonesSetupButton_Click(object sender, RoutedEventArgs e)
    {
        if (_regionSetupWindow is not null)
        {
            _regionSetupWindow.Activate();
            return;
        }

        _restoreOverlayAfterRegionSetup = _overlayController.IsOverlayVisible;
        if (_restoreOverlayAfterRegionSetup)
        {
            _overlayController.HideOverlay();
        }

        _regionSetupWindow = new RegionSetupWindow(_overlayController.RegionBounds, new Size(320, 180));
        _regionSetupWindow.ApplyRequested += RegionSetupWindow_ApplyRequested;
        _regionSetupWindow.Closed += RegionSetupWindow_Closed;
        _regionSetupWindow.Show();
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
        if (_regionSetupWindow is not null)
        {
            _regionSetupWindow.ApplyRequested -= RegionSetupWindow_ApplyRequested;
            _regionSetupWindow.Closed -= RegionSetupWindow_Closed;
            _regionSetupWindow.Close();
            _regionSetupWindow = null;
        }

        if (!Application.Current.Dispatcher.HasShutdownStarted)
        {
            Application.Current.Shutdown();
        }

        base.OnClosing(e);
    }

    private void OverlayController_OverlayStateChanged(object? sender, EventArgs e)
    {
        var regionBoundsChanged = _overlayController.RegionBounds != _lastRegionBounds;
        if (regionBoundsChanged && !_isApplyingPreset)
        {
            _selectedPresetKey = null;
        }

        _lastRegionBounds = _overlayController.RegionBounds;
        UpdateOverlayState();
        UpdateAspectRatioState();
        if (!_isChangingAspectRatio)
        {
            UpdatePresetButtons();
        }
    }

    private void PreviewWindowController_PreviewModeChanged(object? sender, EventArgs e)
    {
        UpdateControlState();
    }

    private void CaptureService_CaptureFailed(object? sender, CaptureFailedEventArgs e)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.Invoke(() => CaptureService_CaptureFailed(sender, e));
            return;
        }

        CaptureErrorText.Text = "Capture stopped: " + e.Exception.Message;
        _previewBlackoutController.RequestBlackout();
        UpdateControlState();
    }

    private void RegionSetupWindow_ApplyRequested(object? sender, EventArgs e)
    {
        if (_regionSetupWindow is null)
        {
            return;
        }

        if (!_overlayController.TryApplyRegionBounds(_regionSetupWindow.GetConfiguredBounds()))
        {
            OverlayStatusText.Text = "Could not apply snap region.";
            return;
        }

        if (_restoreOverlayAfterRegionSetup)
        {
            _overlayController.ShowOverlay();
        }

        _regionSetupWindow.Close();
        UpdateOverlayState();
        _lastRegionBounds = _overlayController.RegionBounds;
        _selectedPresetKey = null;
        UpdatePresetButtons();
    }

    private void RegionSetupWindow_Closed(object? sender, EventArgs e)
    {
        if (_regionSetupWindow is not null)
        {
            _regionSetupWindow.ApplyRequested -= RegionSetupWindow_ApplyRequested;
            _regionSetupWindow.Closed -= RegionSetupWindow_Closed;
            _regionSetupWindow = null;
        }

        if (_restoreOverlayAfterRegionSetup && !_overlayController.IsOverlayVisible)
        {
            _overlayController.ShowOverlay();
        }

        _restoreOverlayAfterRegionSetup = false;
    }

    private void UpdateControlState()
    {
        var state = ControlWindowState.FromState(_captureService.IsCapturing, _previewWindowController.Mode);
        CaptureToggleButton.Content = state.CaptureToggleText;
        CaptureToggleButton.Style = (Style)FindResource(_captureService.IsCapturing ? "DangerButtonStyle" : "PrimaryButtonStyle");
        CaptureStatusText.Text = state.CaptureStatusText;
        CaptureStatusDot.Foreground = GetCaptureStatusBrush(_captureService.IsCapturing);
        CaptureStatusText.Foreground = GetCaptureStatusBrush(_captureService.IsCapturing);
        BorderlessPreviewCheckBox.IsChecked = _previewWindowController.Mode == PreviewWindowMode.Borderless;
        if (_captureService.IsCapturing)
        {
            CaptureErrorText.Text = string.Empty;
        }
    }

    private void UpdateOverlayState()
    {
        var controlState = PreviewOverlayControlState.FromOverlayState(_overlayController.IsLocked, _overlayController.IsOverlayVisible);

        OverlayLockText.Text = controlState.LockToggleText;
        OverlayVisibilityText.Text = controlState.VisibilityToggleText;
        OverlayLockIcon.Text = _overlayController.IsLocked ? "🔓" : "🔒";
        OverlayVisibilityIcon.Text = _overlayController.IsOverlayVisible ? "🙈" : "👁";
        OverlayLockToggleButton.Style = (Style)FindResource(_overlayController.IsLocked ? "WarningButtonStyle" : "SecondaryButtonStyle");
        OverlayStatusText.Text = controlState.StatusText;
    }

    private void UpdateOverlayDisplayOptionsState()
    {
        OverlayStatusCheckBox.IsChecked = _overlayController.IsStatusVisible;
        OverlayLatencyCheckBox.IsChecked = _overlayController.IsLatencyVisible;
    }

    private static Brush GetCaptureStatusBrush(bool isCapturing)
    {
        return (Brush)new BrushConverter().ConvertFromString(isCapturing ? "#DC2626" : "#64748B")!;
    }

    private void UpdateAspectRatioState()
    {
        SetAspectRatioButtonState(AspectFreeButton, AspectRatioMode.Free);
        SetAspectRatioButtonState(AspectSixteenByNineButton, AspectRatioMode.SixteenByNine);
        SetAspectRatioButtonState(AspectSixteenByTenButton, AspectRatioMode.SixteenByTen);
        SetAspectRatioButtonState(AspectFourByThreeButton, AspectRatioMode.FourByThree);
    }

    private void UpdatePresetButtons()
    {
        PresetButtonsPanel.Children.Clear();
        foreach (var preset in PresetSize.ForAspectRatio(_overlayController.AspectRatioMode))
        {
            var button = new System.Windows.Controls.Button
            {
                Content = preset.Name,
                Tag = preset.Key,
                Margin = new Thickness(0, 0, 6, 6),
                Style = (Style)FindResource(_selectedPresetKey == preset.Key ? "SelectedPillButtonStyle" : "PillButtonStyle")
            };
            button.Click += PresetButton_Click;
            PresetButtonsPanel.Children.Add(button);
        }
    }

    private void SetAspectRatioButtonState(System.Windows.Controls.Button button, AspectRatioMode aspectRatioMode)
    {
        button.Style = (Style)FindResource(_overlayController.AspectRatioMode == aspectRatioMode ? "SelectedPillButtonStyle" : "PillButtonStyle");
    }

    private void UpdateCursorCaptureState()
    {
        var state = CursorCaptureControlState.FromEnabled(_cursorCaptureSettings.IsCursorCaptureEnabled);
        ApplyCursorCaptureState(state);
    }

    private void UpdateCaptureFrameRateState()
    {
        var state = CaptureFrameRateControlState.FromFramesPerSecond(_captureFrameRateSettings.FramesPerSecond);
        ApplyCaptureFrameRateState(state);
    }

    private void ApplyCursorCaptureState(CursorCaptureControlState state)
    {
        CursorCaptureCheckBox.IsChecked = state.IsChecked;
        CursorCaptureCheckBox.Content = "Capture Cursor";
    }

    private void ApplyCaptureFrameRateState(CaptureFrameRateControlState state)
    {
        CaptureFrameRateComboBox.SelectedItem = state.FramesPerSecond;
        CaptureFrameRateText.Text = "Capture FPS";
    }
}
