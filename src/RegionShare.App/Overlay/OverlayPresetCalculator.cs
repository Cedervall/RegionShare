using System.Windows;

namespace RegionShare.App.Overlay;

public static class OverlayPresetCalculator
{
    public static Size Apply(Size currentSize, PresetSize presetSize, Size minimumSize, bool isLocked)
    {
        ArgumentNullException.ThrowIfNull(presetSize);

        return new Size(
            Math.Max(presetSize.Width, minimumSize.Width),
            Math.Max(presetSize.Height, minimumSize.Height));
    }
}
