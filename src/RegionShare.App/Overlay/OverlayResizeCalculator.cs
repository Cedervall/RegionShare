using System.Windows;

namespace RegionShare.App.Overlay;

public static class OverlayResizeCalculator
{
    public static Rect Resize(Rect bounds, ResizeHandle handle, double horizontalChange, double verticalChange, Size minimumSize)
    {
        return Resize(bounds, handle, horizontalChange, verticalChange, minimumSize, AspectRatioMode.Free);
    }

    public static Rect Resize(Rect bounds, ResizeHandle handle, double horizontalChange, double verticalChange, Size minimumSize, AspectRatioMode aspectRatioMode)
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

        var resizedBounds = new Rect(left, top, right - left, bottom - top);
        return ConstrainAspectRatio(resizedBounds, bounds, handle, minimumSize, aspectRatioMode);
    }

    private static Rect ConstrainAspectRatio(Rect resizedBounds, Rect originalBounds, ResizeHandle handle, Size minimumSize, AspectRatioMode aspectRatioMode)
    {
        var ratio = AspectRatioCalculator.GetRatio(aspectRatioMode);
        if (ratio is null || handle == ResizeHandle.None)
        {
            return resizedBounds;
        }

        var width = resizedBounds.Width;
        var height = resizedBounds.Height;

        if (ShouldUseHeightAsDriver(resizedBounds, originalBounds, handle))
        {
            width = height * ratio.Value;
        }
        else
        {
            height = width / ratio.Value;
        }

        width = Math.Max(width, minimumSize.Width);
        height = Math.Max(height, minimumSize.Height);

        var left = ResizesLeft(handle) ? originalBounds.Right - width : resizedBounds.Left;
        var top = ResizesTop(handle) ? originalBounds.Bottom - height : resizedBounds.Top;

        return new Rect(left, top, width, height);
    }

    private static bool ShouldUseHeightAsDriver(Rect resizedBounds, Rect originalBounds, ResizeHandle handle)
    {
        var widthChange = Math.Abs(resizedBounds.Width - originalBounds.Width);
        var heightChange = Math.Abs(resizedBounds.Height - originalBounds.Height);
        var isCorner = (ResizesLeft(handle) || ResizesRight(handle)) && (ResizesTop(handle) || ResizesBottom(handle));

        if (isCorner)
        {
            return heightChange > widthChange;
        }

        return ResizesTop(handle) || ResizesBottom(handle);
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
