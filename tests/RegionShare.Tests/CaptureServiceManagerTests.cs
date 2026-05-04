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
        Assert.Equal(ScreenCaptureBackend.Direct3DDesktopDuplication, manager.CurrentBackend);
    }

    [Fact]
    public void StartUsesDirect3DWhenCursorIsEnabledAndGpuIsSupported()
    {
        var cursorSettings = new CursorCaptureSettings { IsCursorCaptureEnabled = true };
        var createdBackends = new List<ScreenCaptureBackend>();
        using var manager = new CaptureServiceManager(cursorSettings, new CaptureFrameRateSettings(), new Support(true), backend =>
        {
            createdBackends.Add(backend);
            return new FakeCaptureService();
        });

        manager.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.Equal([ScreenCaptureBackend.Direct3DDesktopDuplication], createdBackends);
        Assert.Equal(ScreenCaptureBackend.Direct3DDesktopDuplication, manager.CurrentBackend);
    }

    [Fact]
    public void StartUsesGdiWhenGpuIsUnsupported()
    {
        var cursorSettings = new CursorCaptureSettings { IsCursorCaptureEnabled = true };
        var createdBackends = new List<ScreenCaptureBackend>();
        using var manager = new CaptureServiceManager(cursorSettings, new CaptureFrameRateSettings(), new Support(false), backend =>
        {
            createdBackends.Add(backend);
            return new FakeCaptureService();
        });

        manager.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.Equal([ScreenCaptureBackend.Gdi], createdBackends);
        Assert.Equal(ScreenCaptureBackend.Gdi, manager.CurrentBackend);
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
        cursorSettings.IsCursorCaptureEnabled = true;
        manager.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.Equal([ScreenCaptureBackend.Direct3DDesktopDuplication, ScreenCaptureBackend.Direct3DDesktopDuplication], createdBackends);
        Assert.Equal(ScreenCaptureBackend.Direct3DDesktopDuplication, manager.CurrentBackend);
    }

    [Fact]
    public void StopClearsCurrentBackend()
    {
        using var manager = new CaptureServiceManager(new CursorCaptureSettings(), new CaptureFrameRateSettings(), new Support(true), _ => new FakeCaptureService());

        manager.Start(new CaptureRegion(0, 0, 320, 180));
        manager.Stop();

        Assert.Null(manager.CurrentBackend);
    }

    [Fact]
    public void BackendChangedFiresWhenBackendChanges()
    {
        var cursorSettings = new CursorCaptureSettings { IsCursorCaptureEnabled = false };
        using var manager = new CaptureServiceManager(cursorSettings, new CaptureFrameRateSettings(), new Support(true), _ => new FakeCaptureService());
        var changedBackends = new List<ScreenCaptureBackend?>();
        manager.BackendChanged += (_, _) => changedBackends.Add(manager.CurrentBackend);

        manager.Start(new CaptureRegion(0, 0, 320, 180));
        manager.Stop();
        cursorSettings.IsCursorCaptureEnabled = true;
        manager.Start(new CaptureRegion(0, 0, 320, 180));

        Assert.Equal([ScreenCaptureBackend.Direct3DDesktopDuplication, null, ScreenCaptureBackend.Direct3DDesktopDuplication], changedBackends);
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
