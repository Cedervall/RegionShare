using System.Windows;
using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class OverlayPresetCalculatorTests
{
    [Fact]
    public void ApplyReturnsPresetSizeWhenUnlocked()
    {
        var size = OverlayPresetCalculator.Apply(new Size(640, 360), PresetSize.Hd, new Size(320, 180), false);

        Assert.Equal(new Size(1280, 720), size);
    }

    [Fact]
    public void ApplyReturnsPresetSizeWhenLocked()
    {
        var size = OverlayPresetCalculator.Apply(new Size(640, 360), PresetSize.Hd, new Size(320, 180), true);

        Assert.Equal(new Size(1280, 720), size);
    }

    [Fact]
    public void ApplyEnforcesMinimumSizeWhenUnlocked()
    {
        var smallPreset = new PresetSize("Small", 100, 80);

        var size = OverlayPresetCalculator.Apply(new Size(640, 360), smallPreset, new Size(320, 180), false);

        Assert.Equal(new Size(320, 180), size);
    }
}
