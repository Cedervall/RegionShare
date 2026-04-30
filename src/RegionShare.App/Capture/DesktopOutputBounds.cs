namespace RegionShare.App.Capture;

public sealed record DesktopOutputBounds(int Left, int Top, int Right, int Bottom)
{
    public int Width => Right - Left;

    public int Height => Bottom - Top;
}
