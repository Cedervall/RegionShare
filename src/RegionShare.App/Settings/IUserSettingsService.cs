namespace RegionShare.App.Settings;

public interface IUserSettingsService
{
    UserSettings Load();

    void Save(UserSettings settings);
}
