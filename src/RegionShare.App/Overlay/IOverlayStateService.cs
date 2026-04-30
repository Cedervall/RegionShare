namespace RegionShare.App.Overlay;

public interface IOverlayStateService
{
    bool IsLocked { get; }

    AspectRatioMode AspectRatioMode { get; set; }

    void ToggleLock();
}
