namespace RegionShare.App.Windowing;

using System.Windows;

public static class WindowPlacementCalculator
{
    public static Rect EnsureVisible(Rect requestedBounds, Size minimumSize, IReadOnlyList<Rect> workAreas, Point fallbackOffset)
    {
        var width = SanitizeSize(requestedBounds.Width, minimumSize.Width);
        var height = SanitizeSize(requestedBounds.Height, minimumSize.Height);
        var normalizedBounds = new Rect(SanitizeCoordinate(requestedBounds.Left), SanitizeCoordinate(requestedBounds.Top), width, height);

        if (workAreas.Count == 0)
        {
            return normalizedBounds;
        }

        var workArea = FindBestWorkArea(normalizedBounds, workAreas);
        var intersectsWorkArea = workArea.IntersectsWith(normalizedBounds);
        width = Math.Min(width, Math.Max(minimumSize.Width, workArea.Width));
        height = Math.Min(height, Math.Max(minimumSize.Height, workArea.Height));

        var left = intersectsWorkArea ? normalizedBounds.Left : workArea.Left + fallbackOffset.X;
        var top = intersectsWorkArea ? normalizedBounds.Top : workArea.Top + fallbackOffset.Y;

        return new Rect(
            Clamp(left, workArea.Left, workArea.Right - width),
            Clamp(top, workArea.Top, workArea.Bottom - height),
            width,
            height);
    }

    private static Rect FindBestWorkArea(Rect bounds, IReadOnlyList<Rect> workAreas)
    {
        var bestWorkArea = workAreas[0];
        var bestIntersectionArea = -1.0;

        foreach (var workArea in workAreas)
        {
            var intersection = Rect.Intersect(bounds, workArea);
            var intersectionArea = intersection.IsEmpty ? 0 : intersection.Width * intersection.Height;
            if (intersectionArea > bestIntersectionArea)
            {
                bestWorkArea = workArea;
                bestIntersectionArea = intersectionArea;
            }
        }

        return bestWorkArea;
    }

    private static double SanitizeCoordinate(double value)
    {
        return double.IsFinite(value) ? value : 0;
    }

    private static double SanitizeSize(double value, double minimum)
    {
        return double.IsFinite(value) && value >= minimum ? value : minimum;
    }

    private static double Clamp(double value, double minimum, double maximum)
    {
        if (maximum < minimum)
        {
            return minimum;
        }

        return Math.Min(Math.Max(value, minimum), maximum);
    }
}
