using System.Windows;

namespace RegionShare.App.Windows;

public partial class RegionSetupWindow : Window
{
    public RegionSetupWindow(Rect initialBounds, Size minimumSize)
    {
        InitializeComponent();
        MinWidth = minimumSize.Width;
        MinHeight = minimumSize.Height;
        Left = initialBounds.Left;
        Top = initialBounds.Top;
        Width = Math.Max(initialBounds.Width, minimumSize.Width);
        Height = Math.Max(initialBounds.Height, minimumSize.Height);
    }

    public event EventHandler? ApplyRequested;

    public Rect GetConfiguredBounds()
    {
        return new Rect(Left, Top, Width, Height);
    }

    private void ApplyButton_Click(object sender, RoutedEventArgs e)
    {
        ApplyRequested?.Invoke(this, EventArgs.Empty);
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
