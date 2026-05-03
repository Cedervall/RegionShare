using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class PresetSizeTests
{
    [Fact]
    public void AllReturnsSupportedPresetSizesInDisplayOrder()
    {
        Assert.Equal(
            [PresetSize.Hd, PresetSize.HdPlus, PresetSize.FullHd, PresetSize.Qhd, PresetSize.Wxga, PresetSize.WxgaPlus, PresetSize.WsxgaPlus, PresetSize.Wuxga, PresetSize.Xga, PresetSize.Sxga, PresetSize.Uxga],
            PresetSize.All);
    }

    [Theory]
    [InlineData(AspectRatioMode.Free, "1280x720", "1600x900", "1920x1080", "2560x1440")]
    [InlineData(AspectRatioMode.SixteenByNine, "1280x720", "1600x900", "1920x1080", "2560x1440")]
    [InlineData(AspectRatioMode.SixteenByTen, "1280x800", "1440x900", "1680x1050", "1920x1200")]
    [InlineData(AspectRatioMode.FourByThree, "1024x768", "1280x960", "1600x1200")]
    public void ForAspectRatioReturnsMatchingPresets(AspectRatioMode aspectRatioMode, params string[] expectedKeys)
    {
        var presetKeys = PresetSize.ForAspectRatio(aspectRatioMode).Select(preset => preset.Key);

        Assert.Equal(expectedKeys, presetKeys);
    }

    [Fact]
    public void FromKeyReturnsMatchingPreset()
    {
        var preset = PresetSize.FromKey("1920x1200");

        Assert.Equal(PresetSize.Wuxga, preset);
    }

    [Fact]
    public void FromKeyReturnsNullForUnknownPreset()
    {
        var preset = PresetSize.FromKey("123x456");

        Assert.Null(preset);
    }
}
