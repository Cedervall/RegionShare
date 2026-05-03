using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class ControlWindowStateTests
{
    [Theory]
    [InlineData(false, PreviewWindowMode.Normal, "Start Capture", "Status: Stopped", "Use borderless preview")]
    [InlineData(true, PreviewWindowMode.Normal, "Stop Capture", "Status: Capturing", "Use borderless preview")]
    [InlineData(false, PreviewWindowMode.Borderless, "Start Capture", "Status: Stopped", "Use normal preview")]
    public void FromStateReturnsControlLabels(bool isCapturing, PreviewWindowMode mode, string captureText, string statusText, string borderlessText)
    {
        var state = ControlWindowState.FromState(isCapturing, mode);

        Assert.Equal(captureText, state.CaptureToggleText);
        Assert.Equal(statusText, state.CaptureStatusText);
        Assert.Equal(borderlessText, state.BorderlessToggleText);
    }
}
