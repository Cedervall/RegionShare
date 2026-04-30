namespace RegionShare.App.Overlay;

public interface IOverlayController
{
    event EventHandler? OverlayStateChanged;

    bool IsLocked { get; }

    bool IsOverlayVisible { get; }

    void ToggleLock();

    void ShowOverlay();

    void HideOverlay();
}
