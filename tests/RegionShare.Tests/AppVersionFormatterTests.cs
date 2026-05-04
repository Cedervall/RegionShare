using RegionShare.App.Metadata;

namespace RegionShare.Tests;

public sealed class AppVersionFormatterTests
{
    [Theory]
    [InlineData("0.1.2", "Version 0.1.2")]
    [InlineData("0.1.2+abcdef", "Version 0.1.2")]
    [InlineData(" 0.1.2 ", "Version 0.1.2")]
    public void FormatReturnsDisplayVersion(string version, string expected)
    {
        var displayVersion = AppVersionFormatter.Format(version);

        Assert.Equal(expected, displayVersion);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void FormatReturnsUnknownForMissingVersion(string? version)
    {
        var displayVersion = AppVersionFormatter.Format(version);

        Assert.Equal("Version unknown", displayVersion);
    }
}
