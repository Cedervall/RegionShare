using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class CaptureBackendLabelFormatterTests
{
    [Theory]
    [InlineData(ScreenCaptureBackend.Direct3DDesktopDuplication, "GPU powered")]
    [InlineData(ScreenCaptureBackend.Gdi, "Using CPU fallback")]
    public void FormatReturnsBackendLabel(ScreenCaptureBackend backend, string expected)
    {
        var label = CaptureBackendLabelFormatter.Format(backend);

        Assert.Equal(expected, label);
    }

    [Fact]
    public void FormatReturnsNotCapturingForMissingBackend()
    {
        var label = CaptureBackendLabelFormatter.Format(null);

        Assert.Equal("Capture stopped", label);
    }
}
