using System.Windows;

namespace RegionShare.App.Preview;

public static class PreviewPlaceholderState
{
    public static Visibility GetPlaceholderVisibility(bool hasFrame)
    {
        return hasFrame ? Visibility.Collapsed : Visibility.Visible;
    }
}
