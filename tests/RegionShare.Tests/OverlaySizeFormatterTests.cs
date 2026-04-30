using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class OverlaySizeFormatterTests
{
    [Theory]
    [InlineData(640, 360, "640 x 360")]
    [InlineData(1279.6, 719.5, "1280 x 720")]
    [InlineData(320.5, 180.5, "321 x 181")]
    [InlineData(320.4, 180.4, "320 x 180")]
    public void FormatRoundsOverlaySizeForDisplay(double width, double height, string expected)
    {
        var actual = OverlaySizeFormatter.Format(width, height);

        Assert.Equal(expected, actual);
    }
}
