using System.Windows;

namespace RegionShare.App.Overlay;

public static class OverlayResizeCalculator
{
    public static Rect Resize(Rect bounds, ResizeHandle handle, double horizontalChange, double verticalChange, Size minimumSize)
    {
        if (minimumSize.Width <= 0 || minimumSize.Height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(minimumSize), "Minimum size must have positive dimensions.");
        }

        var left = bounds.Left;
        var top = bounds.Top;
        var right = bounds.Right;
        var bottom = bounds.Bottom;

        if (ResizesLeft(handle))
        {
            left = Math.Min(left + horizontalChange, right - minimumSize.Width);
        }

        if (ResizesRight(handle))
        {
            right = Math.Max(right + horizontalChange, left + minimumSize.Width);
        }

        if (ResizesTop(handle))
        {
            top = Math.Min(top + verticalChange, bottom - minimumSize.Height);
        }

        if (ResizesBottom(handle))
        {
            bottom = Math.Max(bottom + verticalChange, top + minimumSize.Height);
        }

        return new Rect(left, top, right - left, bottom - top);
    }

    private static bool ResizesLeft(ResizeHandle handle)
    {
        return handle is ResizeHandle.Left or ResizeHandle.TopLeft or ResizeHandle.BottomLeft;
    }

    private static bool ResizesRight(ResizeHandle handle)
    {
        return handle is ResizeHandle.Right or ResizeHandle.TopRight or ResizeHandle.BottomRight;
    }

    private static bool ResizesTop(ResizeHandle handle)
    {
        return handle is ResizeHandle.Top or ResizeHandle.TopLeft or ResizeHandle.TopRight;
    }

    private static bool ResizesBottom(ResizeHandle handle)
    {
        return handle is ResizeHandle.Bottom or ResizeHandle.BottomLeft or ResizeHandle.BottomRight;
    }
}
