namespace RegionShare.App.Capture;

using System.Runtime.InteropServices;

internal static class NativeCursorRenderer
{
    private const int CursorShowing = 0x00000001;
    private const int DrawNormal = 0x0003;

    public static void DrawCursorIfEnabled(IntPtr targetDc, CaptureRegion region, ICursorCaptureSettings cursorCaptureSettings)
    {
        if (!cursorCaptureSettings.IsCursorCaptureEnabled)
        {
            return;
        }

        var cursorInfo = new CursorInfo
        {
            Size = Marshal.SizeOf<CursorInfo>()
        };

        if (!GetCursorInfo(ref cursorInfo) || (cursorInfo.Flags & CursorShowing) == 0 || cursorInfo.Handle == IntPtr.Zero)
        {
            return;
        }

        var regionPoint = CursorCaptureCalculator.ToRegionPoint(new System.Windows.Point(cursorInfo.ScreenPosition.X, cursorInfo.ScreenPosition.Y), region);
        if (regionPoint is null)
        {
            return;
        }

        if (!GetIconInfo(cursorInfo.Handle, out var iconInfo))
        {
            return;
        }

        try
        {
            var x = (int)Math.Round(regionPoint.Value.X) - iconInfo.HotspotX;
            var y = (int)Math.Round(regionPoint.Value.Y) - iconInfo.HotspotY;
            DrawIconEx(targetDc, x, y, cursorInfo.Handle, 0, 0, 0, IntPtr.Zero, DrawNormal);
        }
        finally
        {
            if (iconInfo.MaskBitmap != IntPtr.Zero)
            {
                DeleteObject(iconInfo.MaskBitmap);
            }

            if (iconInfo.ColorBitmap != IntPtr.Zero)
            {
                DeleteObject(iconInfo.ColorBitmap);
            }
        }
    }

    [DllImport("user32.dll")]
    private static extern bool GetCursorInfo(ref CursorInfo cursorInfo);

    [DllImport("user32.dll")]
    private static extern bool GetIconInfo(IntPtr iconHandle, out IconInfo iconInfo);

    [DllImport("user32.dll")]
    private static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr iconHandle, int width, int height, int animationStep, IntPtr flickerFreeDraw, int flags);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [StructLayout(LayoutKind.Sequential)]
    private struct CursorInfo
    {
        public int Size;
        public int Flags;
        public IntPtr Handle;
        public Point ScreenPosition;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Point
    {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct IconInfo
    {
        public bool IsIcon;
        public int HotspotX;
        public int HotspotY;
        public IntPtr MaskBitmap;
        public IntPtr ColorBitmap;
    }
}
