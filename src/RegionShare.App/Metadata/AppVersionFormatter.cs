namespace RegionShare.App.Metadata;

public static class AppVersionFormatter
{
    public static string Format(string? version)
    {
        if (string.IsNullOrWhiteSpace(version))
        {
            return "Version unknown";
        }

        var trimmedVersion = version.Trim();
        var metadataIndex = trimmedVersion.IndexOf('+', StringComparison.Ordinal);
        var displayVersion = metadataIndex >= 0 ? trimmedVersion[..metadataIndex] : trimmedVersion;

        return $"Version {displayVersion}";
    }
}
