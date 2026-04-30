namespace RegionShare.App.Overlay;

public sealed record OverlayLockVisualState(string BorderBrush, string StatusText, string ToggleText, string SizeToolTip)
{
    public static OverlayLockVisualState FromLockState(bool isLocked)
    {
        return isLocked
            ? new OverlayLockVisualState("#F59E0B", "Locked", "Unlock", "Locked region size")
            : new OverlayLockVisualState("#22C55E", "Unlocked", "Lock", "Unlocked region size");
    }
}
