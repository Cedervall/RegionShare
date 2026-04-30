namespace RegionShare.App.Overlay;

public sealed class OverlayStateService : IOverlayStateService
{
    public bool IsLocked { get; private set; }

    public AspectRatioMode AspectRatioMode { get; set; } = AspectRatioMode.Free;

    public void ToggleLock()
    {
        IsLocked = !IsLocked;
    }
}
