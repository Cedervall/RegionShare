namespace RegionShare.App.Overlay;

public sealed record PresetSize(string Name, int Width, int Height)
{
    public string Key => $"{Width}x{Height}";

    public static IReadOnlyList<PresetSize> All { get; } = [Hd, HdPlus, FullHd, Qhd, Wxga, WxgaPlus, WsxgaPlus, Wuxga, Xga, Sxga, Uxga];

    public static IReadOnlyList<PresetSize> Common { get; } = [Hd, HdPlus, FullHd, Qhd];

    public static IReadOnlyList<PresetSize> SixteenByNine { get; } = [Hd, HdPlus, FullHd, Qhd];

    public static IReadOnlyList<PresetSize> SixteenByTen { get; } = [Wxga, WxgaPlus, WsxgaPlus, Wuxga];

    public static IReadOnlyList<PresetSize> FourByThree { get; } = [Xga, Sxga, Uxga];

    public static PresetSize Hd => new("1280 x 720", 1280, 720);

    public static PresetSize HdPlus => new("1600 x 900", 1600, 900);

    public static PresetSize FullHd => new("1920 x 1080", 1920, 1080);

    public static PresetSize Qhd => new("2560 x 1440", 2560, 1440);

    public static PresetSize Wxga => new("1280 x 800", 1280, 800);

    public static PresetSize WxgaPlus => new("1440 x 900", 1440, 900);

    public static PresetSize WsxgaPlus => new("1680 x 1050", 1680, 1050);

    public static PresetSize Wuxga => new("1920 x 1200", 1920, 1200);

    public static PresetSize Xga => new("1024 x 768", 1024, 768);

    public static PresetSize Sxga => new("1280 x 960", 1280, 960);

    public static PresetSize Uxga => new("1600 x 1200", 1600, 1200);

    public static IReadOnlyList<PresetSize> ForAspectRatio(AspectRatioMode aspectRatioMode)
    {
        return aspectRatioMode switch
        {
            AspectRatioMode.SixteenByNine => SixteenByNine,
            AspectRatioMode.SixteenByTen => SixteenByTen,
            AspectRatioMode.FourByThree => FourByThree,
            _ => Common
        };
    }

    public static PresetSize? FromKey(string key)
    {
        return All.FirstOrDefault(preset => string.Equals(preset.Key, key, StringComparison.Ordinal));
    }
}
