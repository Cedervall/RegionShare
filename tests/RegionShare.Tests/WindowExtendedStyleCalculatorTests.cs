using RegionShare.App.Windowing;

namespace RegionShare.Tests;

public sealed class WindowExtendedStyleCalculatorTests
{
    [Fact]
    public void SetClickThroughAddsTransparentStyle()
    {
        var style = WindowExtendedStyleCalculator.SetClickThrough(0, true);

        Assert.Equal(WindowExtendedStyleCalculator.Transparent, style & WindowExtendedStyleCalculator.Transparent);
    }

    [Fact]
    public void SetClickThroughKeepsExistingStylesWhenAddingTransparentStyle()
    {
        const int existingStyle = 0x00000080;

        var style = WindowExtendedStyleCalculator.SetClickThrough(existingStyle, true);

        Assert.Equal(existingStyle, style & existingStyle);
        Assert.Equal(WindowExtendedStyleCalculator.Transparent, style & WindowExtendedStyleCalculator.Transparent);
    }

    [Fact]
    public void SetClickThroughRemovesTransparentStyleOnly()
    {
        const int existingStyle = 0x00000080;
        var currentStyle = existingStyle | WindowExtendedStyleCalculator.Transparent;

        var style = WindowExtendedStyleCalculator.SetClickThrough(currentStyle, false);

        Assert.Equal(existingStyle, style);
        Assert.Equal(0, style & WindowExtendedStyleCalculator.Transparent);
    }
}
