namespace RegionShare.App.Capture;

using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SharpGen.Runtime;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.Mathematics;

public sealed class Direct3DDesktopDuplicationScreenCaptureService : IScreenCaptureService, IDisposable
{
    private readonly ICaptureFrameRateSettings _captureFrameRateSettings;
    private readonly object _syncRoot = new();
    private CaptureRegion? _region;
    private CancellationTokenSource? _captureCancellation;
    private Task? _captureTask;
    private bool _isDisposed;

    public Direct3DDesktopDuplicationScreenCaptureService(ICaptureFrameRateSettings captureFrameRateSettings)
    {
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
            _captureTask = Task.Run(() => CaptureLoopAsync(region, _captureCancellation.Token));
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

    private async Task CaptureLoopAsync(CaptureRegion region, CancellationToken cancellationToken)
    {
        try
        {
            using var session = Direct3DDesktopDuplicationSession.Create(region);
            using var timer = new PeriodicTimer(CaptureFrameRateCalculator.ToInterval(_captureFrameRateSettings.FramesPerSecond));
            while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
            {
                timer.Period = CaptureFrameRateCalculator.ToInterval(_captureFrameRateSettings.FramesPerSecond);
                var frame = session.TryCaptureFrame();
                if (frame is not null)
                {
                    FrameCaptured?.Invoke(this, new CapturedFrameEventArgs(frame, Stopwatch.GetTimestamp()));
                }
            }
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception exception) when (exception is InvalidOperationException or ExternalException or SharpGenException)
        {
            Stop();
            CaptureFailed?.Invoke(this, new CaptureFailedEventArgs(exception));
        }
    }

    private void StopCaptureLoop()
    {
        _captureCancellation?.Cancel();
        _captureCancellation = null;
        _captureTask = null;
        IsCapturing = false;
    }

    private sealed class Direct3DDesktopDuplicationSession : IDisposable
    {
        private readonly CaptureRegion _relativeRegion;
        private readonly ID3D11Device _device;
        private readonly ID3D11DeviceContext _context;
        private readonly IDXGIOutputDuplication _duplication;
        private readonly ID3D11Texture2D _stagingTexture;
        private readonly byte[] _pixels;
        private bool _frameAcquired;

        private Direct3DDesktopDuplicationSession(CaptureRegion relativeRegion, ID3D11Device device, ID3D11DeviceContext context, IDXGIOutputDuplication duplication, ID3D11Texture2D stagingTexture)
        {
            _relativeRegion = relativeRegion;
            _device = device;
            _context = context;
            _duplication = duplication;
            _stagingTexture = stagingTexture;
            _pixels = new byte[relativeRegion.Width * relativeRegion.Height * 4];
        }

        public static Direct3DDesktopDuplicationSession Create(CaptureRegion region)
        {
            using var factory = DXGI.CreateDXGIFactory1<IDXGIFactory1>();
            var outputSelection = Direct3DOutputSelector.Select(factory, region) ?? throw new InvalidOperationException("Capture region must fit within one attached display output for GPU capture.");

            var device = D3D11.D3D11CreateDevice(DriverType.Hardware, DeviceCreationFlags.BgraSupport);
            var context = device.ImmediateContext;

            var output1 = outputSelection.Output.QueryInterface<IDXGIOutput1>();
            var duplication = output1.DuplicateOutput(device);
            output1.Dispose();

            var stagingTexture = CreateStagingTexture(device, outputSelection.MappedRegion.RelativeRegion);
            outputSelection.Dispose();
            return new Direct3DDesktopDuplicationSession(outputSelection.MappedRegion.RelativeRegion, device, context, duplication, stagingTexture);
        }

        public BitmapSource? TryCaptureFrame()
        {
            IDXGIResource? desktopResource = null;
            try
            {
                var acquireResult = _duplication.AcquireNextFrame(0, out _, out desktopResource);
                if (acquireResult.Code == Vortice.DXGI.ResultCode.WaitTimeout.Code)
                {
                    return null;
                }

                acquireResult.CheckError();
                _frameAcquired = true;

                using var desktopTexture = desktopResource.QueryInterface<ID3D11Texture2D>();
                var sourceBox = new Box(
                    _relativeRegion.X,
                    _relativeRegion.Y,
                    0,
                    _relativeRegion.X + _relativeRegion.Width,
                    _relativeRegion.Y + _relativeRegion.Height,
                    1);
                _context.CopySubresourceRegion(_stagingTexture, 0, 0, 0, 0, desktopTexture, 0, sourceBox);

                return CreateBitmapSourceFromStagingTexture();
            }
            finally
            {
                desktopResource?.Dispose();
                if (_frameAcquired)
                {
                    _duplication.ReleaseFrame();
                    _frameAcquired = false;
                }
            }
        }

        public void Dispose()
        {
            _stagingTexture.Dispose();
            _duplication.Dispose();
            _context.Dispose();
            _device.Dispose();
        }

        private static ID3D11Texture2D CreateStagingTexture(ID3D11Device device, CaptureRegion relativeRegion)
        {
            var description = new Texture2DDescription(
                Format.B8G8R8A8_UNorm,
                (uint)relativeRegion.Width,
                (uint)relativeRegion.Height,
                1,
                1,
                BindFlags.None,
                ResourceUsage.Staging,
                CpuAccessFlags.Read,
                1,
                0,
                ResourceOptionFlags.None);

            return device.CreateTexture2D(description);
        }

        private BitmapSource CreateBitmapSourceFromStagingTexture()
        {
            _context.Map(_stagingTexture, 0, MapMode.Read, Vortice.Direct3D11.MapFlags.None, out var mappedResource).CheckError();
            try
            {
                var targetStride = _relativeRegion.Width * 4;
                for (var y = 0; y < _relativeRegion.Height; y++)
                {
                    var source = IntPtr.Add(mappedResource.DataPointer, y * (int)mappedResource.RowPitch);
                    Marshal.Copy(source, _pixels, y * targetStride, targetStride);
                }
            }
            finally
            {
                _context.Unmap(_stagingTexture, 0);
            }

            var bitmap = BitmapSource.Create(_relativeRegion.Width, _relativeRegion.Height, 96, 96, PixelFormats.Bgra32, null, _pixels, _relativeRegion.Width * 4);
            bitmap.Freeze();
            return bitmap;
        }
    }
}
