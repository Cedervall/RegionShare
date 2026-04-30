namespace RegionShare.App.Overlay;

public sealed record PresetSize(string Name, int Width, int Height)
{
    public static IReadOnlyList<PresetSize> All { get; } = [Hd, HdPlus, FullHd];

    public static PresetSize Hd => new("1280 x 720", 1280, 720);

    public static PresetSize HdPlus => new("1600 x 900", 1600, 900);

    public static PresetSize FullHd => new("1920 x 1080", 1920, 1080);
}
