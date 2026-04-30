using RegionShare.App.Capture;

namespace RegionShare.App.Preview;

public static class CursorCaptureToggleController
{
    public static CursorCaptureControlState Apply(bool isChecked, ICursorCaptureSettings cursorCaptureSettings)
    {
        ArgumentNullException.ThrowIfNull(cursorCaptureSettings);

        cursorCaptureSettings.IsCursorCaptureEnabled = isChecked;
        return CursorCaptureControlState.FromEnabled(cursorCaptureSettings.IsCursorCaptureEnabled);
    }
}
