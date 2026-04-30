using RegionShare.App.Capture;
using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class CaptureFrameRateToggleControllerTests
{
    [Fact]
    public void ApplyUpdatesFrameRateSettingsAndReturnsLabel()
    {
        var settings = new CaptureFrameRateSettings();

        var state = CaptureFrameRateToggleController.Apply(120, settings);

        Assert.Equal(120, settings.FramesPerSecond);
        Assert.Equal(120, state.FramesPerSecond);
        Assert.Equal("Capture FPS: 120", state.Label);
    }

    [Fact]
    public void ApplyFallsBackToDefaultForUnsupportedFrameRate()
    {
        var settings = new CaptureFrameRateSettings
        {
            FramesPerSecond = 120
        };

        var state = CaptureFrameRateToggleController.Apply(144, settings);

        Assert.Equal(60, settings.FramesPerSecond);
        Assert.Equal(60, state.FramesPerSecond);
        Assert.Equal("Capture FPS: 60", state.Label);
    }
}
