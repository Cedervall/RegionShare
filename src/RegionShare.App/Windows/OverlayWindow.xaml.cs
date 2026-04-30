using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);

        if (_overlayState.IsLocked)
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
        if (_overlayState.IsLocked || sender is not Thumb thumb)
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

    private void UpdateSizeText()
    {
        if (SizeText is null)
        {
            return;
        }

        SizeText.Text = OverlaySizeFormatter.Format(ActualWidth, ActualHeight);
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
