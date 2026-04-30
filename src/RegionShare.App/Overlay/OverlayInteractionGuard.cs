namespace RegionShare.App.Overlay;

public static class OverlayInteractionGuard
{
    public static bool CanMove(IOverlayStateService overlayState)
    {
        ArgumentNullException.ThrowIfNull(overlayState);

        return !overlayState.IsLocked;
    }

    public static bool CanResize(IOverlayStateService overlayState)
    {
        ArgumentNullException.ThrowIfNull(overlayState);

        return !overlayState.IsLocked;
    }
}
