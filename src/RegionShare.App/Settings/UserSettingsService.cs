namespace RegionShare.App.Settings;

using System.IO;
using System.Text.Json;

public sealed class UserSettingsService : IUserSettingsService
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _settingsPath;

    public UserSettingsService()
        : this(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegionShare", "settings.json"))
    {
    }

    public UserSettingsService(string settingsPath)
    {
        _settingsPath = settingsPath;
    }

    public UserSettings Load()
    {
        if (!File.Exists(_settingsPath))
        {
            return UserSettings.Default;
        }

        try
        {
            var json = File.ReadAllText(_settingsPath);
            var settings = JsonSerializer.Deserialize<UserSettings>(json, SerializerOptions) ?? UserSettings.Default;
            return UserSettingsValidator.Sanitize(settings);
        }
        catch (JsonException)
        {
            return UserSettings.Default;
        }
        catch (IOException)
        {
            return UserSettings.Default;
        }
    }

    public void Save(UserSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        var directory = Path.GetDirectoryName(_settingsPath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }

        var json = JsonSerializer.Serialize(settings, SerializerOptions);
        File.WriteAllText(_settingsPath, json);
    }
}
