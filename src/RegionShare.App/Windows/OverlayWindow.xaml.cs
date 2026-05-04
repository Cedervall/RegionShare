using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using RegionShare.App.Capture;
using RegionShare.App.Dpi;
using RegionShare.App.Overlay;
using RegionShare.App.Preview;
using RegionShare.App.Windowing;

namespace RegionShare.App.Windows;

public partial class OverlayWindow : Window, IOverlayController
{
    private readonly IOverlayStateService _overlayState;
    private readonly IDpiService _dpiService;
    private readonly IWindowCaptureExclusionService _captureExclusionService;
    private readonly IWindowClickThroughService _clickThroughService;
    private readonly IFrameTimingTelemetry _frameTimingTelemetry;
    private readonly ICaptureBackendStatus _captureBackendStatus;
    private bool _isStatusVisible = true;
    private bool _isLatencyVisible = true;

    public OverlayWindow(IOverlayStateService overlayState, IDpiService dpiService, IWindowCaptureExclusionService captureExclusionService, IWindowClickThroughService clickThroughService, IFrameTimingTelemetry frameTimingTelemetry, ICaptureBackendStatus captureBackendStatus)
    {
        _overlayState = overlayState;
        _dpiService = dpiService;
        _captureExclusionService = captureExclusionService;
        _clickThroughService = clickThroughService;
        _frameTimingTelemetry = frameTimingTelemetry;
        _captureBackendStatus = captureBackendStatus;
        InitializeComponent();
        FrameTimingText.Text = FrameTimingLabelFormatter.Format(_frameTimingTelemetry.Current);
        CaptureBackendText.Text = CaptureBackendLabelFormatter.Format(_captureBackendStatus.CurrentBackend);
        _frameTimingTelemetry.TimingUpdated += FrameTimingTelemetry_TimingUpdated;
        _captureBackendStatus.BackendChanged += CaptureBackendStatus_BackendChanged;
        UpdateSizeText();
        UpdateLockVisualState();
    }

    public event EventHandler? OverlayStateChanged;

    public bool IsLocked => _overlayState.IsLocked;

    public bool IsOverlayVisible => IsVisible;

    public bool IsStatusVisible => _isStatusVisible;

    public bool IsLatencyVisible => _isLatencyVisible;

    public AspectRatioMode AspectRatioMode => _overlayState.AspectRatioMode;

    public Rect RegionBounds => new(Left, Top, Width, Height);

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);
        _captureExclusionService.ExcludeFromCapture(this);
        _clickThroughService.SetClickThrough(this, _overlayState.IsLocked);
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        if (!OverlayInteractionGuard.CanMove(_overlayState))
        {
            return;
        }

        DragMove();
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);
        UpdateSizeText();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (Application.Current.Dispatcher.HasShutdownStarted)
        {
            base.OnClosing(e);
            return;
        }

        e.Cancel = true;
        HideOverlay();
    }

    protected override void OnClosed(EventArgs e)
    {
        _frameTimingTelemetry.TimingUpdated -= FrameTimingTelemetry_TimingUpdated;
        _captureBackendStatus.BackendChanged -= CaptureBackendStatus_BackendChanged;
        base.OnClosed(e);
    }

    public CaptureRegion GetCaptureRegion()
    {
        var dpiScale = PresentationSource.FromVisual(this)?.CompositionTarget?.TransformToDevice ?? Matrix.Identity;
        return _dpiService.ToPhysicalRegion(new Rect(Left, Top, Width, Height), dpiScale.M11, dpiScale.M22);
    }

    public void ToggleLock()
    {
        _overlayState.ToggleLock();
        UpdateLockVisualState();
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ShowOverlay()
    {
        Show();
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void HideOverlay()
    {
        Hide();
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ToggleOverlayVisibility()
    {
        if (IsOverlayVisible)
        {
            HideOverlay();
            return;
        }

        ShowOverlay();
    }

    public void ApplyPreset(PresetSize presetSize)
    {
        var nextSize = OverlayPresetCalculator.Apply(
            new Size(Width, Height),
            presetSize,
            new Size(MinWidth, MinHeight),
            _overlayState.IsLocked);

        if (nextSize.Width.Equals(Width) && nextSize.Height.Equals(Height))
        {
            return;
        }

        Width = nextSize.Width;
        Height = nextSize.Height;
        UpdateSizeText();
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TryApplyRegionBounds(Rect bounds)
    {
        var appliedBounds = RegionSetupBoundsCalculator.Apply(bounds, new Size(MinWidth, MinHeight), _overlayState.IsLocked);
        if (appliedBounds.IsEmpty)
        {
            return false;
        }

        Left = appliedBounds.Left;
        Top = appliedBounds.Top;
        Width = appliedBounds.Width;
        Height = appliedBounds.Height;
        _overlayState.AspectRatioMode = AspectRatioMode.Free;
        UpdateSizeText();
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
        return true;
    }

    public void SetAspectRatioMode(AspectRatioMode aspectRatioMode)
    {
        _overlayState.AspectRatioMode = aspectRatioMode;
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetStatusVisibility(bool isVisible)
    {
        _isStatusVisible = isVisible;
        UpdateOverlayInfoVisibility();
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetLatencyVisibility(bool isVisible)
    {
        _isLatencyVisible = isVisible;
        UpdateOverlayInfoVisibility();
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ResizeHandle_DragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!OverlayInteractionGuard.CanResize(_overlayState) || sender is not Thumb thumb)
        {
            return;
        }

        var handle = GetResizeHandle(thumb.Name);
        var bounds = new Rect(Left, Top, Width, Height);
        var minimumSize = new Size(MinWidth, MinHeight);
        _overlayState.AspectRatioMode = AspectRatioMode.Free;
        var resizedBounds = OverlayResizeCalculator.Resize(bounds, handle, e.HorizontalChange, e.VerticalChange, minimumSize, AspectRatioMode.Free);

        Left = resizedBounds.Left;
        Top = resizedBounds.Top;
        Width = resizedBounds.Width;
        Height = resizedBounds.Height;
        OverlayStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void LockToggle_Click(object sender, RoutedEventArgs e)
    {
        ToggleLock();
    }

    private void FrameTimingTelemetry_TimingUpdated(object? sender, FrameTimingSample e)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.BeginInvoke(() => FrameTimingTelemetry_TimingUpdated(sender, e));
            return;
        }

        FrameTimingText.Text = FrameTimingLabelFormatter.Format(e);
    }

    private void CaptureBackendStatus_BackendChanged(object? sender, EventArgs e)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.BeginInvoke(() => CaptureBackendStatus_BackendChanged(sender, e));
            return;
        }

        CaptureBackendText.Text = CaptureBackendLabelFormatter.Format(_captureBackendStatus.CurrentBackend);
    }

    private void UpdateSizeText()
    {
        if (SizeText is null)
        {
            return;
        }

        SizeText.Text = OverlaySizeFormatter.Format(ActualWidth, ActualHeight);
    }

    private void UpdateLockVisualState()
    {
        var visualState = OverlayLockVisualState.FromLockState(_overlayState.IsLocked);

        FrameBorder.BorderBrush = (Brush)new BrushConverter().ConvertFromString(visualState.BorderBrush)!;
        LockStatusText.Text = visualState.StatusText;
        LockContextMenuItem.Header = _overlayState.IsLocked ? "Unlock region" : "Lock region";
        SizeText.ToolTip = visualState.SizeToolTip;
        SetResizeHandlesEnabled(!_overlayState.IsLocked);
        _clickThroughService.SetClickThrough(this, _overlayState.IsLocked);
        UpdateOverlayInfoVisibility();
    }

    private void UpdateOverlayInfoVisibility()
    {
        var statusVisibility = _isStatusVisible ? Visibility.Visible : Visibility.Collapsed;
        SizeText.Visibility = statusVisibility;
        LockStatusText.Visibility = statusVisibility;
        CaptureBackendText.Visibility = statusVisibility;
        FrameTimingText.Visibility = _isLatencyVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    private void SetResizeHandlesEnabled(bool isEnabled)
    {
        TopLeftResizeHandle.IsEnabled = isEnabled;
        TopResizeHandle.IsEnabled = isEnabled;
        TopRightResizeHandle.IsEnabled = isEnabled;
        RightResizeHandle.IsEnabled = isEnabled;
        BottomRightResizeHandle.IsEnabled = isEnabled;
        BottomResizeHandle.IsEnabled = isEnabled;
        BottomLeftResizeHandle.IsEnabled = isEnabled;
        LeftResizeHandle.IsEnabled = isEnabled;
    }

    private static ResizeHandle GetResizeHandle(string name)
    {
        return name switch
        {
            nameof(TopLeftResizeHandle) => ResizeHandle.TopLeft,
            nameof(TopResizeHandle) => ResizeHandle.Top,
            nameof(TopRightResizeHandle) => ResizeHandle.TopRight,
            nameof(RightResizeHandle) => ResizeHandle.Right,
            nameof(BottomRightResizeHandle) => ResizeHandle.BottomRight,
            nameof(BottomResizeHandle) => ResizeHandle.Bottom,
            nameof(BottomLeftResizeHandle) => ResizeHandle.BottomLeft,
            nameof(LeftResizeHandle) => ResizeHandle.Left,
            _ => ResizeHandle.None
        };
    }
}
