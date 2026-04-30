using System.Windows;
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

    private void UpdateSizeText()
    {
        if (SizeText is null)
        {
            return;
        }

        SizeText.Text = $"{ActualWidth:0} x {ActualHeight:0}";
    }
}
