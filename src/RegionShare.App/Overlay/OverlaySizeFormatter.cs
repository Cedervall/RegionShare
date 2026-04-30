namespace RegionShare.App.Overlay;

using System.Globalization;

public static class OverlaySizeFormatter
{
    public static string Format(double width, double height)
    {
        return $"{width.ToString("0", CultureInfo.InvariantCulture)} x {height.ToString("0", CultureInfo.InvariantCulture)}";
    }
}
