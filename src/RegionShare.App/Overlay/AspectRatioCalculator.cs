using System.Windows;

namespace RegionShare.App.Overlay;

public static class AspectRatioCalculator
{
    public static double? GetRatio(AspectRatioMode aspectRatioMode)
    {
        return aspectRatioMode switch
        {
            AspectRatioMode.SixteenByNine => 16.0 / 9.0,
            AspectRatioMode.SixteenByTen => 16.0 / 10.0,
            AspectRatioMode.FourByThree => 4.0 / 3.0,
            _ => null
        };
    }

    public static Size Constrain(Size size, AspectRatioMode aspectRatioMode)
    {
        var ratio = GetRatio(aspectRatioMode);
        if (ratio is null || size.Width <= 0 || size.Height <= 0)
        {
            return size;
        }

        var widthFromHeight = size.Height * ratio.Value;
        var heightFromWidth = size.Width / ratio.Value;

        return Math.Abs(widthFromHeight - size.Width) < Math.Abs(heightFromWidth - size.Height)
            ? new Size(widthFromHeight, size.Height)
            : new Size(size.Width, heightFromWidth);
    }
}
