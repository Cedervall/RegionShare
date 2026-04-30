using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewWindowControllerTests
{
    [Fact]
    public void SetModeUpdatesModeAndRaisesEvent()
    {
        var controller = new PreviewWindowController();
        var eventCount = 0;
        controller.PreviewModeChanged += (_, _) => eventCount++;

        controller.SetMode(PreviewWindowMode.Borderless);

        Assert.Equal(PreviewWindowMode.Borderless, controller.Mode);
        Assert.Equal(1, eventCount);
    }

    [Fact]
    public void SetModeDoesNotRaiseEventWhenModeDoesNotChange()
    {
        var controller = new PreviewWindowController();
        var eventCount = 0;
        controller.PreviewModeChanged += (_, _) => eventCount++;

        controller.SetMode(PreviewWindowMode.Normal);

        Assert.Equal(0, eventCount);
    }
}
