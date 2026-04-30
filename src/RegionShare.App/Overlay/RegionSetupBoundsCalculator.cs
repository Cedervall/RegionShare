namespace RegionShare.App.Overlay;

using System.Windows;

public static class RegionSetupBoundsCalculator
{
    public static Rect Apply(Rect requestedBounds, Size minimumSize, bool isLocked)
    {
        if (isLocked)
        {
            return Rect.Empty;
        }

        var width = Math.Max(requestedBounds.Width, minimumSize.Width);
        var height = Math.Max(requestedBounds.Height, minimumSize.Height);
        return new Rect(requestedBounds.Left, requestedBounds.Top, width, height);
    }
}
