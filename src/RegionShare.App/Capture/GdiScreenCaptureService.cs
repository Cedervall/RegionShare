namespace RegionShare.App.Capture;

using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

public sealed class GdiScreenCaptureService : IScreenCaptureService, IDisposable
{
    private const int CopySource = 0x00CC0020;
    private const int CursorShowing = 0x00000001;
    private const int DrawNormal = 0x0003;
    private static readonly TimeSpan CaptureInterval = TimeSpan.FromMilliseconds(33);
    private readonly ICursorCaptureSettings _cursorCaptureSettings;
    private readonly DispatcherTimer _captureTimer;
    private CaptureRegion? _region;
    private bool _isDisposed;

    public GdiScreenCaptureService()
        : this(new CursorCaptureSettings())
    {
    }

    public GdiScreenCaptureService(ICursorCaptureSettings cursorCaptureSettings)
    {
        _cursorCaptureSettings = cursorCaptureSettings;
        _captureTimer = new DispatcherTimer
        {
            Interval = CaptureInterval
        };
        _captureTimer.Tick += OnCaptureTimerTick;
    }

    public event EventHandler<CapturedFrameEventArgs>? FrameCaptured;

    public event EventHandler<CaptureFailedEventArgs>? CaptureFailed;

    public bool IsCapturing { get; private set; }

    public void Start(CaptureRegion region)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        if (!region.IsValid)
        {
            throw new ArgumentOutOfRangeException(nameof(region), "Capture region must have positive dimensions.");
        }

        _region = region;
        IsCapturing = true;
        _captureTimer.Start();
    }

    public void Stop()
    {
        _captureTimer.Stop();
        IsCapturing = false;
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        _captureTimer.Stop();
        _captureTimer.Tick -= OnCaptureTimerTick;
        IsCapturing = false;
        _isDisposed = true;
    }

    private void OnCaptureTimerTick(object? sender, EventArgs e)
    {
        CaptureFramePump.CaptureNextFrame(
            IsCapturing,
            _region,
            CaptureFrame,
            frame => FrameCaptured?.Invoke(this, new CapturedFrameEventArgs(frame)),
            Stop,
            exception => CaptureFailed?.Invoke(this, new CaptureFailedEventArgs(exception)));
    }

    private BitmapSource CaptureFrame(CaptureRegion region)
    {
        var screenDc = GetDC(IntPtr.Zero);
        if (screenDc == IntPtr.Zero)
        {
            throw new InvalidOperationException("Unable to get screen device context.");
        }

        var memoryDc = IntPtr.Zero;
        var bitmap = IntPtr.Zero;
        var previousObject = IntPtr.Zero;

        try
        {
            memoryDc = CreateCompatibleDC(screenDc);
            if (memoryDc == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create compatible device context.");
            }

            bitmap = CreateCompatibleBitmap(screenDc, region.Width, region.Height);
            if (bitmap == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create compatible bitmap.");
            }

            previousObject = SelectObject(memoryDc, bitmap);
            if (!BitBlt(memoryDc, 0, 0, region.Width, region.Height, screenDc, region.X, region.Y, CopySource))
            {
                throw new InvalidOperationException("Unable to copy screen region.");
            }

            DrawCursorIfEnabled(memoryDc, region);

            var source = Imaging.CreateBitmapSourceFromHBitmap(bitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            source.Freeze();
            return source;
        }
        finally
        {
            if (previousObject != IntPtr.Zero && memoryDc != IntPtr.Zero)
            {
                SelectObject(memoryDc, previousObject);
            }

            if (bitmap != IntPtr.Zero)
            {
                DeleteObject(bitmap);
            }

            if (memoryDc != IntPtr.Zero)
            {
                DeleteDC(memoryDc);
            }

            ReleaseDC(IntPtr.Zero, screenDc);
        }
    }

    private void DrawCursorIfEnabled(IntPtr targetDc, CaptureRegion region)
    {
        if (!_cursorCaptureSettings.IsCursorCaptureEnabled)
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
    private static extern IntPtr GetDC(IntPtr hwnd);

    [DllImport("user32.dll")]
    private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int width, int height);

    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int width, int height, IntPtr hdcSource, int xSource, int ySource, int rasterOperation);

    [DllImport("user32.dll")]
    private static extern bool GetCursorInfo(ref CursorInfo cursorInfo);

    [DllImport("user32.dll")]
    private static extern bool GetIconInfo(IntPtr iconHandle, out IconInfo iconInfo);

    [DllImport("user32.dll")]
    private static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr iconHandle, int width, int height, int animationStep, IntPtr flickerFreeDraw, int flags);

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
