namespace RegionShare.App.Capture;

public sealed class CaptureServiceManager : IScreenCaptureService, IDisposable
{
    private readonly ICursorCaptureSettings _cursorCaptureSettings;
    private readonly ICaptureFrameRateSettings _captureFrameRateSettings;
    private readonly IScreenCaptureBackendSupport _backendSupport;
    private readonly Func<ScreenCaptureBackend, IScreenCaptureService> _serviceFactory;
    private readonly object _syncRoot = new();
    private IScreenCaptureService? _currentService;
    private bool _isDisposed;

    public CaptureServiceManager(ICursorCaptureSettings cursorCaptureSettings, ICaptureFrameRateSettings captureFrameRateSettings, IScreenCaptureBackendSupport backendSupport)
        : this(cursorCaptureSettings, captureFrameRateSettings, backendSupport, backend => backend switch
        {
            ScreenCaptureBackend.Direct3DDesktopDuplication => new Direct3DDesktopDuplicationScreenCaptureService(captureFrameRateSettings),
            _ => new GdiScreenCaptureService(cursorCaptureSettings, captureFrameRateSettings)
        })
    {
    }

    public CaptureServiceManager(ICursorCaptureSettings cursorCaptureSettings, ICaptureFrameRateSettings captureFrameRateSettings, IScreenCaptureBackendSupport backendSupport, Func<ScreenCaptureBackend, IScreenCaptureService> serviceFactory)
    {
        _cursorCaptureSettings = cursorCaptureSettings;
        _captureFrameRateSettings = captureFrameRateSettings;
        _backendSupport = backendSupport;
        _serviceFactory = serviceFactory;
    }

    public event EventHandler<CapturedFrameEventArgs>? FrameCaptured;

    public event EventHandler<CaptureFailedEventArgs>? CaptureFailed;

    public bool IsCapturing => _currentService?.IsCapturing == true;

    public void Start(CaptureRegion region)
    {
        ObjectDisposedException.ThrowIf(_isDisposed, this);

        lock (_syncRoot)
        {
            StopCurrentService();
            var backend = ScreenCaptureBackendSelector.Select(_cursorCaptureSettings.IsCursorCaptureEnabled, _backendSupport.IsDirect3DDesktopDuplicationSupported);
            var service = _serviceFactory(backend);
            service.FrameCaptured += CurrentService_FrameCaptured;
            service.CaptureFailed += CurrentService_CaptureFailed;
            _currentService = service;
            try
            {
                service.Start(region);
            }
            catch
            {
                StopCurrentService();
                throw;
            }
        }
    }

    public void Stop()
    {
        lock (_syncRoot)
        {
            StopCurrentService();
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
            StopCurrentService();
        }

        _isDisposed = true;
    }

    private void StopCurrentService()
    {
        if (_currentService is null)
        {
            return;
        }

        _currentService.FrameCaptured -= CurrentService_FrameCaptured;
        _currentService.CaptureFailed -= CurrentService_CaptureFailed;
        _currentService.Stop();
        if (_currentService is IDisposable disposable)
        {
            disposable.Dispose();
        }

        _currentService = null;
    }

    private void CurrentService_FrameCaptured(object? sender, CapturedFrameEventArgs e)
    {
        FrameCaptured?.Invoke(this, e);
    }

    private void CurrentService_CaptureFailed(object? sender, CaptureFailedEventArgs e)
    {
        CaptureFailed?.Invoke(this, e);
    }
}
