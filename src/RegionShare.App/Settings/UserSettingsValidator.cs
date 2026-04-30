namespace RegionShare.App.Settings;

using RegionShare.App.Capture;
using RegionShare.App.Overlay;

public static class UserSettingsValidator
{
    private const double MinimumOverlayWidth = 320;
    private const double MinimumOverlayHeight = 180;
    private const double MinimumPreviewWidth = 320;
    private const double MinimumPreviewHeight = 180;
    private const double MinimumControlWidth = 420;
    private const double MinimumControlHeight = 260;

    public static UserSettings Sanitize(UserSettings settings)
    {
        return settings with
        {
            OverlayLeft = SanitizeCoordinate(settings.OverlayLeft, UserSettings.Default.OverlayLeft),
            OverlayTop = SanitizeCoordinate(settings.OverlayTop, UserSettings.Default.OverlayTop),
            OverlayWidth = SanitizeSize(settings.OverlayWidth, MinimumOverlayWidth, UserSettings.Default.OverlayWidth),
            OverlayHeight = SanitizeSize(settings.OverlayHeight, MinimumOverlayHeight, UserSettings.Default.OverlayHeight),
            AspectRatioMode = Enum.IsDefined(settings.AspectRatioMode) ? settings.AspectRatioMode : AspectRatioMode.Free,
            PreviewLeft = SanitizeCoordinate(settings.PreviewLeft, UserSettings.Default.PreviewLeft),
            PreviewTop = SanitizeCoordinate(settings.PreviewTop, UserSettings.Default.PreviewTop),
            PreviewWidth = SanitizeSize(settings.PreviewWidth, MinimumPreviewWidth, UserSettings.Default.PreviewWidth),
            PreviewHeight = SanitizeSize(settings.PreviewHeight, MinimumPreviewHeight, UserSettings.Default.PreviewHeight),
            ControlLeft = SanitizeCoordinate(settings.ControlLeft, UserSettings.Default.ControlLeft),
            ControlTop = SanitizeCoordinate(settings.ControlTop, UserSettings.Default.ControlTop),
            ControlWidth = SanitizeSize(settings.ControlWidth, MinimumControlWidth, UserSettings.Default.ControlWidth),
            ControlHeight = SanitizeSize(settings.ControlHeight, MinimumControlHeight, UserSettings.Default.ControlHeight),
            CaptureFramesPerSecond = CaptureFrameRateCalculator.Sanitize(settings.CaptureFramesPerSecond)
        };
    }

    private static double SanitizeCoordinate(double value, double fallback)
    {
        return double.IsFinite(value) ? value : fallback;
    }

    private static double SanitizeSize(double value, double minimum, double fallback)
    {
        return double.IsFinite(value) && value >= minimum ? value : fallback;
    }
}
