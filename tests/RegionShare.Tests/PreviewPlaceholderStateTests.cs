using System.Windows;
using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewPlaceholderStateTests
{
    [Fact]
    public void PlaceholderIsVisibleBeforeFirstFrame()
    {
        Assert.Equal(Visibility.Visible, PreviewPlaceholderState.GetPlaceholderVisibility(false));
    }

    [Fact]
    public void PlaceholderIsCollapsedAfterFirstFrame()
    {
        Assert.Equal(Visibility.Collapsed, PreviewPlaceholderState.GetPlaceholderVisibility(true));
    }
}
