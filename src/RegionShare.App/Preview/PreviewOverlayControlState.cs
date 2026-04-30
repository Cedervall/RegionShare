namespace RegionShare.App.Preview;

public sealed record PreviewOverlayControlState(string LockToggleText, string VisibilityToggleText, string StatusText)
{
    public static PreviewOverlayControlState FromOverlayState(bool isLocked, bool isVisible)
    {
        return new PreviewOverlayControlState(
            isLocked ? "Unlock overlay" : "Lock overlay",
            isVisible ? "Hide overlay" : "Show overlay",
            $"Overlay {(isLocked ? "locked" : "unlocked")}, {(isVisible ? "visible" : "hidden")}");
    }
}
