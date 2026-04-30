using System.Windows;
using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewFitCalculatorTests
{
    [Fact]
    public void FitPreservesAspectRatioWhenAvailableAreaIsWider()
    {
        var fitted = PreviewFitCalculator.Fit(new Size(1920, 1080), new Size(1000, 1000));

        Assert.Equal(1000, fitted.Width, 6);
        Assert.Equal(562.5, fitted.Height, 6);
    }

    [Fact]
    public void FitPreservesAspectRatioWhenAvailableAreaIsTaller()
    {
        var fitted = PreviewFitCalculator.Fit(new Size(1920, 1080), new Size(800, 300));

        Assert.Equal(533.333333, fitted.Width, 6);
        Assert.Equal(300, fitted.Height, 6);
    }

    [Fact]
    public void FitReturnsEmptySizeForInvalidInput()
    {
        var fitted = PreviewFitCalculator.Fit(new Size(0, 1080), new Size(800, 300));

        Assert.Equal(Size.Empty, fitted);
    }
}
