using System.Windows;
using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class AspectRatioCalculatorTests
{
    [Theory]
    [InlineData(AspectRatioMode.Free, null)]
    [InlineData(AspectRatioMode.SixteenByNine, 16.0 / 9.0)]
    [InlineData(AspectRatioMode.SixteenByTen, 16.0 / 10.0)]
    [InlineData(AspectRatioMode.FourByThree, 4.0 / 3.0)]
    public void GetRatioReturnsExpectedRatio(AspectRatioMode mode, double? expected)
    {
        Assert.Equal(expected, AspectRatioCalculator.GetRatio(mode));
    }

    [Fact]
    public void ConstrainReturnsOriginalSizeForFreeMode()
    {
        var size = AspectRatioCalculator.Constrain(new Size(1000, 700), AspectRatioMode.Free);

        Assert.Equal(new Size(1000, 700), size);
    }

    [Fact]
    public void ConstrainAdjustsClosestDimensionToRatio()
    {
        var size = AspectRatioCalculator.Constrain(new Size(1600, 1000), AspectRatioMode.SixteenByNine);

        Assert.Equal(1600, size.Width);
        Assert.Equal(900, size.Height);
    }
}
