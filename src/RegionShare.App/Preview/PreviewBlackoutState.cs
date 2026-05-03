namespace RegionShare.App.Preview;

using System.Windows;

public sealed record PreviewBlackoutState(object? PreviewSource, Visibility PlaceholderVisibility)
{
    public static PreviewBlackoutState Blackout { get; } = new(null, Visibility.Visible);
}
