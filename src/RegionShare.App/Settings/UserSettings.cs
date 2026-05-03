using RegionShare.App.Overlay;

namespace RegionShare.App.Settings;

public sealed record UserSettings(
    double OverlayLeft,
    double OverlayTop,
    double OverlayWidth,
    double OverlayHeight,
    bool IsOverlayVisible,
    bool IsLocked,
    AspectRatioMode AspectRatioMode,
    double PreviewLeft,
    double PreviewTop,
    double PreviewWidth,
    double PreviewHeight,
    bool IsPreviewBorderless,
    double ControlLeft,
    double ControlTop,
    double ControlWidth,
    double ControlHeight,
    bool IsCursorCaptureEnabled,
    int CaptureFramesPerSecond,
    bool? IsOverlayStatusVisible,
    bool? IsOverlayLatencyVisible)
{
    public static UserSettings Default { get; } = new(
        100,
        100,
        1280,
        720,
        true,
        false,
        AspectRatioMode.Free,
        160,
        160,
        960,
        540,
        false,
        220,
        220,
        520,
        560,
        false,
        60,
        true,
        true);
}
