using System.Windows;

namespace RegionShare.App.Capture;

public static class CursorCaptureCalculator
{
    public static Point? ToRegionPoint(Point cursorScreenPoint, CaptureRegion region)
    {
        var relativeX = cursorScreenPoint.X - region.X;
        var relativeY = cursorScreenPoint.Y - region.Y;

        if (relativeX < 0 || relativeY < 0 || relativeX >= region.Width || relativeY >= region.Height)
        {
            return null;
        }

        return new Point(relativeX, relativeY);
    }
}
