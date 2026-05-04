using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class ScreenCaptureServiceFactoryTests
{
    [Fact]
    public void CreateReturnsDirect3DServiceWhenSupportedAndCursorCaptureIsDisabled()
    {
        var service = ScreenCaptureServiceFactory.Create(new CursorCaptureSettings(), new CaptureFrameRateSettings(), new StubBackendSupport(true));

        Assert.IsType<Direct3DDesktopDuplicationScreenCaptureService>(service);
    }

    [Fact]
    public void CreateReturnsGdiServiceWhenDirect3DIsUnsupported()
    {
        var service = ScreenCaptureServiceFactory.Create(new CursorCaptureSettings(), new CaptureFrameRateSettings(), new StubBackendSupport(false));

        Assert.IsType<GdiScreenCaptureService>(service);
    }

    [Fact]
    public void CreateReturnsDirect3DServiceWhenCursorCaptureIsEnabledAndDirect3DIsSupported()
    {
        var service = ScreenCaptureServiceFactory.Create(new CursorCaptureSettings { IsCursorCaptureEnabled = true }, new CaptureFrameRateSettings(), new StubBackendSupport(true));

        Assert.IsType<Direct3DDesktopDuplicationScreenCaptureService>(service);
    }

    private sealed class StubBackendSupport : IScreenCaptureBackendSupport
    {
        public StubBackendSupport(bool isSupported)
        {
            IsDirect3DDesktopDuplicationSupported = isSupported;
        }

        public bool IsDirect3DDesktopDuplicationSupported { get; }
    }
}
