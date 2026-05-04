namespace RegionShare.App.Capture;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

public sealed class GdiScreenCaptureService : IScreenCaptureService, IDisposable
{
    private const int CopySource = 0x00CC0020;
    private readonly ICursorCaptureSettings _cursorCaptureSettings;
    private readonly ICaptureFrameRateSettings _captureFrameRateSettings;
    private readonly object _syncRoot = new();
    private CaptureRegion? _region;
    private CancellationTokenSource? _captureCancellation;
    private Task? _captureTask;
    private bool _isDisposed;

    public GdiScreenCaptureService()
        : this(new CursorCaptureSettings(), new CaptureFrameRateSettings())
    {
    }

    public GdiScreenCaptureService(ICursorCaptureSettings cursorCaptureSettings)
        : this(cursorCaptureSettings, new CaptureFrameRateSettings())
    {
    }

    public GdiScreenCaptureService(ICursorCaptureSettings cursorCaptureSettings, ICaptureFrameRateSettings captureFrameRateSettings)
    {
        _cursorCaptureSettings = cursorCaptureSettings;
        _captureFrameRateSettings = captureFrameRateSettings;
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

        lock (_syncRoot)
        {
            StopCaptureLoop();
            _region = region;
            IsCapturing = true;
            _captureCancellation = new CancellationTokenSource();
            _captureTask = Task.Run(() => CaptureLoopAsync(_captureCancellation.Token));
        }
    }

    public void Stop()
    {
        lock (_syncRoot)
        {
            StopCaptureLoop();
        }
    }

    public void Dispose()
    {
        if (_isDisposed)
        {
            return;
        }

        lock (_syncRoot)
        {
            StopCaptureLoop();
        }

        _isDisposed = true;
    }

    private async Task CaptureLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var captureBuffer = new GdiCaptureBuffer();
            using var timer = new PeriodicTimer(CaptureFrameRateCalculator.ToInterval(_captureFrameRateSettings.FramesPerSecond));
            while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
            {
                timer.Period = CaptureFrameRateCalculator.ToInterval(_captureFrameRateSettings.FramesPerSecond);
                CaptureNextFrame(captureBuffer);
            }
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void CaptureNextFrame(GdiCaptureBuffer captureBuffer)
    {
        CaptureFramePump.CaptureNextFrame(
            IsCapturing,
            _region,
            region => CaptureFrame(region, captureBuffer),
            frame => FrameCaptured?.Invoke(this, new CapturedFrameEventArgs(frame, Stopwatch.GetTimestamp())),
            Stop,
            exception => CaptureFailed?.Invoke(this, new CaptureFailedEventArgs(exception)));
    }

    private void StopCaptureLoop()
    {
        _captureCancellation?.Cancel();
        _captureCancellation = null;
        _captureTask = null;
        IsCapturing = false;
    }

    private BitmapSource CaptureFrame(CaptureRegion region, GdiCaptureBuffer captureBuffer)
    {
        captureBuffer.Ensure(region);
        if (!BitBlt(captureBuffer.MemoryDc, 0, 0, region.Width, region.Height, captureBuffer.ScreenDc, region.X, region.Y, CopySource))
        {
            throw new InvalidOperationException("Unable to copy screen region.");
        }

        NativeCursorRenderer.DrawCursorIfEnabled(captureBuffer.MemoryDc, region, _cursorCaptureSettings);

        var source = Imaging.CreateBitmapSourceFromHBitmap(captureBuffer.Bitmap, IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        source.Freeze();
        return source;
    }

    private sealed class GdiCaptureBuffer : IDisposable
    {
        private int _width;
        private int _height;
        private IntPtr _previousObject;

        public IntPtr ScreenDc { get; private set; }

        public IntPtr MemoryDc { get; private set; }

        public IntPtr Bitmap { get; private set; }

        public void Ensure(CaptureRegion region)
        {
            EnsureScreenDc();
            EnsureMemoryDc();
            EnsureBitmap(region);
        }

        public void Dispose()
        {
            ReleaseBitmap();

            if (MemoryDc != IntPtr.Zero)
            {
                DeleteDC(MemoryDc);
                MemoryDc = IntPtr.Zero;
            }

            if (ScreenDc != IntPtr.Zero)
            {
                ReleaseDC(IntPtr.Zero, ScreenDc);
                ScreenDc = IntPtr.Zero;
            }
        }

        private void EnsureScreenDc()
        {
            if (ScreenDc != IntPtr.Zero)
            {
                return;
            }

            ScreenDc = GetDC(IntPtr.Zero);
            if (ScreenDc == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to get screen device context.");
            }
        }

        private void EnsureMemoryDc()
        {
            if (MemoryDc != IntPtr.Zero)
            {
                return;
            }

            MemoryDc = CreateCompatibleDC(ScreenDc);
            if (MemoryDc == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create compatible device context.");
            }
        }

        private void EnsureBitmap(CaptureRegion region)
        {
            if (Bitmap != IntPtr.Zero && !GdiCaptureResourcePlan.ShouldRecreateBitmap(_width, _height, region))
            {
                return;
            }

            ReleaseBitmap();

            Bitmap = CreateCompatibleBitmap(ScreenDc, region.Width, region.Height);
            if (Bitmap == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create compatible bitmap.");
            }

            _previousObject = SelectObject(MemoryDc, Bitmap);
            _width = region.Width;
            _height = region.Height;
        }

        private void ReleaseBitmap()
        {
            if (_previousObject != IntPtr.Zero && MemoryDc != IntPtr.Zero)
            {
                SelectObject(MemoryDc, _previousObject);
                _previousObject = IntPtr.Zero;
            }

            if (Bitmap != IntPtr.Zero)
            {
                DeleteObject(Bitmap);
                Bitmap = IntPtr.Zero;
            }

            _width = 0;
            _height = 0;
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

}
