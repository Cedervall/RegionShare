using System.Windows;
using RegionShare.App.Capture;

namespace RegionShare.App.Dpi;

public sealed class DpiService : IDpiService
{
    public CaptureRegion ToPhysicalRegion(Rect logicalRegion, double dpiScaleX, double dpiScaleY)
    {
        ValidateScale(dpiScaleX, nameof(dpiScaleX));
        ValidateScale(dpiScaleY, nameof(dpiScaleY));

        return new CaptureRegion(
            ToPhysicalPixel(logicalRegion.X, dpiScaleX),
            ToPhysicalPixel(logicalRegion.Y, dpiScaleY),
            ToPhysicalPixel(logicalRegion.Width, dpiScaleX),
            ToPhysicalPixel(logicalRegion.Height, dpiScaleY));
    }

    private static int ToPhysicalPixel(double logicalValue, double dpiScale)
    {
        return (int)Math.Round(logicalValue * dpiScale, MidpointRounding.AwayFromZero);
    }

    private static void ValidateScale(double dpiScale, string parameterName)
    {
        if (dpiScale <= 0 || double.IsNaN(dpiScale) || double.IsInfinity(dpiScale))
        {
            throw new ArgumentOutOfRangeException(parameterName, "DPI scale must be a positive finite value.");
        }
    }
}
