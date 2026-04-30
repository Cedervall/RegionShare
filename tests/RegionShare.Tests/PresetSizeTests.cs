using RegionShare.App.Overlay;

namespace RegionShare.Tests;

public sealed class PresetSizeTests
{
    [Fact]
    public void AllReturnsSupportedPresetSizesInDisplayOrder()
    {
        Assert.Equal(
            [PresetSize.Hd, PresetSize.HdPlus, PresetSize.FullHd],
            PresetSize.All);
    }
}
