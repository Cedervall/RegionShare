using System.Windows;
using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewWindowModeStateTests
{
    [Fact]
    public void FromModeReturnsNormalWindowState()
    {
        var state = PreviewWindowModeState.FromMode(PreviewWindowMode.Normal);

        Assert.Equal(WindowStyle.SingleBorderWindow, state.WindowStyle);
        Assert.Equal(ResizeMode.CanResize, state.ResizeMode);
        Assert.Equal(new Thickness(0), state.ContentMargin);
    }

    [Fact]
    public void FromModeReturnsBorderlessWindowState()
    {
        var state = PreviewWindowModeState.FromMode(PreviewWindowMode.Borderless);

        Assert.Equal(WindowStyle.None, state.WindowStyle);
        Assert.Equal(ResizeMode.CanResize, state.ResizeMode);
        Assert.Equal(new Thickness(0), state.ContentMargin);
    }
}
