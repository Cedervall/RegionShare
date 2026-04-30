using RegionShare.App.Overlay;
using RegionShare.App.Settings;

namespace RegionShare.Tests;

public sealed class UserSettingsValidatorTests
{
    [Fact]
    public void SanitizeKeepsValidSettings()
    {
        var settings = UserSettings.Default with
        {
            OverlayLeft = -1200,
            AspectRatioMode = AspectRatioMode.SixteenByNine,
            IsPreviewBorderless = true
        };

        var sanitized = UserSettingsValidator.Sanitize(settings);

        Assert.Equal(settings, sanitized);
    }

    [Fact]
    public void SanitizeReplacesInvalidSizesAndEnumValues()
    {
        var settings = UserSettings.Default with
        {
            OverlayWidth = 0,
            OverlayHeight = double.PositiveInfinity,
            PreviewWidth = double.NaN,
            ControlHeight = -1,
            AspectRatioMode = (AspectRatioMode)999
        };

        var sanitized = UserSettingsValidator.Sanitize(settings);

        Assert.Equal(UserSettings.Default.OverlayWidth, sanitized.OverlayWidth);
        Assert.Equal(UserSettings.Default.OverlayHeight, sanitized.OverlayHeight);
        Assert.Equal(UserSettings.Default.PreviewWidth, sanitized.PreviewWidth);
        Assert.Equal(UserSettings.Default.ControlHeight, sanitized.ControlHeight);
        Assert.Equal(AspectRatioMode.Free, sanitized.AspectRatioMode);
    }
}
