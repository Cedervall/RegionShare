namespace RegionShare.App.Windowing;

public static class WindowExtendedStyleCalculator
{
    public const int Transparent = 0x00000020;

    public static int SetClickThrough(int extendedStyle, bool isClickThrough)
    {
        return isClickThrough
            ? extendedStyle | Transparent
            : extendedStyle & ~Transparent;
    }
}
