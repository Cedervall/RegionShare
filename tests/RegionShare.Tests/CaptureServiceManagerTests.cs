using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class CaptureServiceManagerTests
{
    [Fact]
    public void StartUsesDirect3DWhenCursorIsDisabledAndGpuIsSupported()
    {
        var cursorSettings = new CursorCaptureSettings { IsCursorCaptureEnabled = false };
        var createdBackends = new List<ScreenCaptureBackend>();
        using var manager = new CaptureServiceManager(cursorSettings, new CaptureFrameRateSettings(), new Support(true), backend =>
        {
            createdBackends.Add(backend);
            return new FakeCaptureService();
        });

        manager.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.Equal([ScreenCaptureBackend.Direct3DDesktopDuplication], createdBackends);
    }

    [Fact]
    public void StartUsesGdiWhenCursorIsEnabled()
    {
        var cursorSettings = new CursorCaptureSettings { IsCursorCaptureEnabled = true };
        var createdBackends = new List<ScreenCaptureBackend>();
        using var manager = new CaptureServiceManager(cursorSettings, new CaptureFrameRateSettings(), new Support(true), backend =>
        {
            createdBackends.Add(backend);
            return new FakeCaptureService();
        });

        manager.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.Equal([ScreenCaptureBackend.Gdi], createdBackends);
    }

    [Fact]
    public void RestartUsesLatestCursorSetting()
    {
        var cursorSettings = new CursorCaptureSettings { IsCursorCaptureEnabled = false };
        var createdBackends = new List<ScreenCaptureBackend>();
        using var manager = new CaptureServiceManager(cursorSettings, new CaptureFrameRateSettings(), new Support(true), backend =>
        {
            createdBackends.Add(backend);
            return new FakeCaptureService();
        });

        manager.Start(new CaptureRegion(0, 0, 320, 180));
        manager.Stop();
        cursorSettings.IsCursorCaptureEnabled = true;
        manager.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.Equal([ScreenCaptureBackend.Direct3DDesktopDuplication, ScreenCaptureBackend.Gdi], createdBackends);
    }

    private sealed class Support(bool isSupported) : IScreenCaptureBackendSupport
    {
        public bool IsDirect3DDesktopDuplicationSupported => isSupported;
    }

    private sealed class FakeCaptureService : IScreenCaptureService, IDisposable
    {
        public event EventHandler<CapturedFrameEventArgs>? FrameCaptured
        {
            add { }
            remove { }
        }

        public event EventHandler<CaptureFailedEventArgs>? CaptureFailed
        {
            add { }
            remove { }
        }

        public bool IsCapturing { get; private set; }

        public void Start(CaptureRegion region)
        {
            IsCapturing = true;
        }

        public void Stop()
        {
            IsCapturing = false;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
