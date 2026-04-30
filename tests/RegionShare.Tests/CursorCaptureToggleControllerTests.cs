using RegionShare.App.Capture;
using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class CursorCaptureToggleControllerTests
{
    [Fact]
    public void ApplyEnablesCursorCaptureAndReturnsOnState()
    {
        var settings = new CursorCaptureSettings();

        var state = CursorCaptureToggleController.Apply(true, settings);

        Assert.True(settings.IsCursorCaptureEnabled);
        Assert.True(state.IsChecked);
        Assert.Equal("Capture cursor: on", state.Label);
    }

    [Fact]
    public void ApplyDisablesCursorCaptureAndReturnsOffState()
    {
        var settings = new CursorCaptureSettings
        {
            IsCursorCaptureEnabled = true
        };

        var state = CursorCaptureToggleController.Apply(false, settings);

        Assert.False(settings.IsCursorCaptureEnabled);
        Assert.False(state.IsChecked);
        Assert.Equal("Capture cursor: off", state.Label);
    }
}
