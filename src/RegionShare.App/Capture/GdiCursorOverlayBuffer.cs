namespace RegionShare.App.Capture;

using System.Runtime.InteropServices;

internal sealed class GdiCursorOverlayBuffer : IDisposable
{
    private const int BitmapInfoHeaderSize = 40;
    private const int ColorPlanes = 1;
    private const int BitsPerPixel = 32;
    private const int BiRgb = 0;
    private const int DibRgbColors = 0;
    private IntPtr _memoryDc;
    private IntPtr _bitmap;
    private IntPtr _bits;
    private IntPtr _previousObject;
    private int _width;
    private int _height;

    public void DrawCursor(byte[] pixels, CaptureRegion region, ICursorCaptureSettings cursorCaptureSettings)
    {
        if (!cursorCaptureSettings.IsCursorCaptureEnabled)
        {
            return;
        }

        Ensure(region.Width, region.Height);
        Marshal.Copy(pixels, 0, _bits, pixels.Length);
        NativeCursorRenderer.DrawCursorIfEnabled(_memoryDc, region, cursorCaptureSettings);
        Marshal.Copy(_bits, pixels, 0, pixels.Length);
    }

    public void Dispose()
    {
        ReleaseBitmap();

        if (_memoryDc != IntPtr.Zero)
        {
            DeleteDC(_memoryDc);
            _memoryDc = IntPtr.Zero;
        }
    }

    private void Ensure(int width, int height)
    {
        if (_memoryDc == IntPtr.Zero)
        {
            _memoryDc = CreateCompatibleDC(IntPtr.Zero);
            if (_memoryDc == IntPtr.Zero)
            {
                throw new InvalidOperationException("Unable to create cursor overlay device context.");
            }
        }

        if (_bitmap != IntPtr.Zero && _width == width && _height == height)
        {
            return;
        }

        ReleaseBitmap();

        var bitmapInfo = new BitmapInfo
        {
            Header = new BitmapInfoHeader
            {
                Size = BitmapInfoHeaderSize,
                Width = width,
                Height = -height,
                Planes = ColorPlanes,
                BitCount = BitsPerPixel,
                Compression = BiRgb,
                SizeImage = width * height * 4
            }
        };

        _bitmap = CreateDIBSection(_memoryDc, ref bitmapInfo, DibRgbColors, out _bits, IntPtr.Zero, 0);
        if (_bitmap == IntPtr.Zero || _bits == IntPtr.Zero)
        {
            throw new InvalidOperationException("Unable to create cursor overlay bitmap.");
        }

        _previousObject = SelectObject(_memoryDc, _bitmap);
        _width = width;
        _height = height;
    }

    private void ReleaseBitmap()
    {
        if (_previousObject != IntPtr.Zero && _memoryDc != IntPtr.Zero)
        {
            SelectObject(_memoryDc, _previousObject);
            _previousObject = IntPtr.Zero;
        }

        if (_bitmap != IntPtr.Zero)
        {
            DeleteObject(_bitmap);
            _bitmap = IntPtr.Zero;
            _bits = IntPtr.Zero;
        }

        _width = 0;
        _height = 0;
    }

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteDC(IntPtr hdc);

    [DllImport("gdi32.dll")]
    private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern bool DeleteObject(IntPtr hObject);

    [DllImport("gdi32.dll")]
    private static extern IntPtr CreateDIBSection(IntPtr hdc, ref BitmapInfo bitmapInfo, int usage, out IntPtr bits, IntPtr section, int offset);

    [StructLayout(LayoutKind.Sequential)]
    private struct BitmapInfo
    {
        public BitmapInfoHeader Header;
        public int Colors;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct BitmapInfoHeader
    {
        public int Size;
        public int Width;
        public int Height;
        public short Planes;
        public short BitCount;
        public int Compression;
        public int SizeImage;
        public int XPelsPerMeter;
        public int YPelsPerMeter;
        public int ClrUsed;
        public int ClrImportant;
    }
}
