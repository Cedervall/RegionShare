namespace RegionShare.App.Overlay;

public interface IOverlayController
{
    event EventHandler? OverlayStateChanged;

    bool IsLocked { get; }

    bool IsOverlayVisible { get; }

    AspectRatioMode AspectRatioMode { get; }

    void ToggleLock();

    void ShowOverlay();

    void HideOverlay();

    void ToggleOverlayVisibility();

    void ApplyPreset(PresetSize presetSize);

    void SetAspectRatioMode(AspectRatioMode aspectRatioMode);
}
