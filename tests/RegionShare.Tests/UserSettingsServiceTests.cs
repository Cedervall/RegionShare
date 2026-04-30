using System.Text.Json;
using RegionShare.App.Overlay;
using RegionShare.App.Settings;

namespace RegionShare.Tests;

public sealed class UserSettingsServiceTests
{
    [Fact]
    public void LoadReturnsDefaultsWhenSettingsFileDoesNotExist()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "settings.json");
        var service = new UserSettingsService(path);

        var settings = service.Load();

        Assert.Equal(UserSettings.Default, settings);
    }

    [Fact]
    public void SaveAndLoadRoundTripsSettings()
    {
        var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"), "settings.json");
        var service = new UserSettingsService(path);
        var settings = UserSettings.Default with
        {
            OverlayLeft = -1200,
            OverlayTop = 80,
            OverlayWidth = 1600,
            OverlayHeight = 900,
            IsOverlayVisible = false,
            IsLocked = true,
            AspectRatioMode = AspectRatioMode.SixteenByNine,
            IsPreviewBorderless = true,
            CaptureFramesPerSecond = 120
        };

        service.Save(settings);

        Assert.Equal(settings, service.Load());
    }

    [Fact]
    public void LoadReturnsDefaultsWhenSettingsFileIsInvalidJson()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, "settings.json");
        File.WriteAllText(path, "not-json");
        var service = new UserSettingsService(path);

        var settings = service.Load();

        Assert.Equal(UserSettings.Default, settings);
    }

    [Fact]
    public void LoadSanitizesValidJsonWithUnsafeWindowValues()
    {
        var directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        var path = Path.Combine(directory, "settings.json");
        var unsafeSettings = UserSettings.Default with
        {
            OverlayWidth = 1,
            PreviewHeight = -10,
            ControlWidth = 0,
            AspectRatioMode = (AspectRatioMode)999
        };
        File.WriteAllText(path, JsonSerializer.Serialize(unsafeSettings));
        var service = new UserSettingsService(path);

        var settings = service.Load();

        Assert.Equal(UserSettings.Default.OverlayWidth, settings.OverlayWidth);
        Assert.Equal(UserSettings.Default.PreviewHeight, settings.PreviewHeight);
        Assert.Equal(UserSettings.Default.ControlWidth, settings.ControlWidth);
        Assert.Equal(AspectRatioMode.Free, settings.AspectRatioMode);
    }

    [Fact]
    public void SerializedSettingsDoNotIncludeCaptureRunningOrScreenContentState()
    {
        var json = JsonSerializer.Serialize(UserSettings.Default);

        Assert.DoesNotContain("CaptureRunning", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Screenshot", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("ScreenContent", json, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("CursorPosition", json, StringComparison.OrdinalIgnoreCase);
    }
}
