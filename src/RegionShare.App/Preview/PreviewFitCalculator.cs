using System.Windows;

namespace RegionShare.App.Preview;

public static class PreviewFitCalculator
{
    public static Size Fit(Size contentSize, Size availableSize)
    {
        if (contentSize.Width <= 0 || contentSize.Height <= 0 || availableSize.Width <= 0 || availableSize.Height <= 0)
        {
            return Size.Empty;
        }

        var scale = Math.Min(availableSize.Width / contentSize.Width, availableSize.Height / contentSize.Height);

        return new Size(contentSize.Width * scale, contentSize.Height * scale);
    }
}
