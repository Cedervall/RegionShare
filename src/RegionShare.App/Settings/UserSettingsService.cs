namespace RegionShare.App.Settings;

public sealed class UserSettingsService : IUserSettingsService
{
    public UserSettings Load()
    {
        return new UserSettings(100, 100, 1280, 720, false, Overlay.AspectRatioMode.Free);
    }

    public void Save(UserSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);
    }
}
