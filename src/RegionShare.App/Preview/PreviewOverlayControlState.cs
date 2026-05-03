namespace RegionShare.App.Preview;

public sealed record PreviewOverlayControlState(string LockToggleText, string VisibilityToggleText, string StatusText)
{
    public static PreviewOverlayControlState FromOverlayState(bool isLocked, bool isVisible)
    {
        return new PreviewOverlayControlState(
            isLocked ? "Unlock Overlay" : "Lock Overlay",
            isVisible ? "Hide Overlay" : "Show Overlay",
            $"Overlay: {(isLocked ? "Locked" : "Unlocked")} & {(isVisible ? "Visible" : "Hidden")}");
    }
}
