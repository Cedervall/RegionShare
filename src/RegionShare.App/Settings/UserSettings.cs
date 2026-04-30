using RegionShare.App.Overlay;

namespace RegionShare.App.Settings;

public sealed record UserSettings(
    double OverlayLeft,
    double OverlayTop,
    double OverlayWidth,
    double OverlayHeight,
    bool IsLocked,
    AspectRatioMode AspectRatioMode);
