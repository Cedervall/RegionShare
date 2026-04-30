using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class CursorCaptureControlStateTests
{
    [Fact]
    public void FromEnabledReturnsUncheckedStateWhenDisabled()
    {
        var state = CursorCaptureControlState.FromEnabled(false);

        Assert.False(state.IsChecked);
        Assert.Equal("Capture cursor: off", state.Label);
    }

    [Fact]
    public void FromEnabledReturnsCheckedStateWhenEnabled()
    {
        var state = CursorCaptureControlState.FromEnabled(true);

        Assert.True(state.IsChecked);
        Assert.Equal("Capture cursor: on", state.Label);
    }
}
