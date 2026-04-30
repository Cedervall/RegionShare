using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using RegionShare.App.Overlay;

namespace RegionShare.App.Windows;

public partial class OverlayWindow : Window
{
    private readonly IOverlayStateService _overlayState;

    public OverlayWindow(IOverlayStateService overlayState)
    {
        _overlayState = overlayState;
        InitializeComponent();
        UpdateSizeText();
        UpdateLockVisualState();
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

    private void ResizeHandle_DragDelta(object sender, DragDeltaEventArgs e)
    {
        if (!OverlayInteractionGuard.CanResize(_overlayState) || sender is not Thumb thumb)
        {
            return;
        }

        var handle = GetResizeHandle(thumb.Name);
        var bounds = new Rect(Left, Top, Width, Height);
        var minimumSize = new Size(MinWidth, MinHeight);
        var resizedBounds = OverlayResizeCalculator.Resize(bounds, handle, e.HorizontalChange, e.VerticalChange, minimumSize);

        Left = resizedBounds.Left;
        Top = resizedBounds.Top;
        Width = resizedBounds.Width;
        Height = resizedBounds.Height;
    }

    private void LockToggle_Click(object sender, RoutedEventArgs e)
    {
        _overlayState.ToggleLock();
        UpdateLockVisualState();
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
        LockToggleButton.Content = visualState.ToggleText;
        LockStatusText.Text = visualState.StatusText;
        LockContextMenuItem.Header = _overlayState.IsLocked ? "Unlock region" : "Lock region";
        SizeText.ToolTip = visualState.SizeToolTip;
        SetResizeHandlesEnabled(!_overlayState.IsLocked);
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
