using System.Windows;
using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewBlackoutStateTests
{
    [Fact]
    public void BlackoutClearsPreviewAndShowsPlaceholder()
    {
        var state = PreviewBlackoutState.Blackout;

        Assert.Null(state.PreviewSource);
        Assert.Equal(Visibility.Visible, state.PlaceholderVisibility);
    }
}
