namespace RegionShare.App.Overlay;

using System.Windows;

public interface IOverlayController
{
    event EventHandler? OverlayStateChanged;

    bool IsLocked { get; }

    bool IsOverlayVisible { get; }

    AspectRatioMode AspectRatioMode { get; }

    Rect RegionBounds { get; }

    void ToggleLock();

    void ShowOverlay();

    void HideOverlay();

    void ToggleOverlayVisibility();

    void ApplyPreset(PresetSize presetSize);

    bool TryApplyRegionBounds(Rect bounds);

    void SetAspectRatioMode(AspectRatioMode aspectRatioMode);
}
