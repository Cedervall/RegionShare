namespace RegionShare.App.Preview;

public sealed record CursorCaptureControlState(bool IsChecked, string Label)
{
    public static CursorCaptureControlState FromEnabled(bool isEnabled)
    {
        return new CursorCaptureControlState(isEnabled, isEnabled ? "Capture cursor: on" : "Capture cursor: off");
    }
}
