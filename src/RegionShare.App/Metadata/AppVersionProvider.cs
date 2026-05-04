using System.Reflection;

namespace RegionShare.App.Metadata;

public static class AppVersionProvider
{
    public static string GetDisplayVersion(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);

        var informationalVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;

        return AppVersionFormatter.Format(informationalVersion ?? assembly.GetName().Version?.ToString());
    }
}
