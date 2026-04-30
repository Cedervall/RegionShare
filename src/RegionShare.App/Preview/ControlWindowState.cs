namespace RegionShare.App.Preview;

public sealed record ControlWindowState(string CaptureToggleText, string CaptureStatusText, string BorderlessToggleText)
{
    public static ControlWindowState FromState(bool isCapturing, PreviewWindowMode previewMode)
    {
        return new ControlWindowState(
            isCapturing ? "Stop capture" : "Start capture",
            isCapturing ? "Capturing" : "Stopped",
            previewMode == PreviewWindowMode.Borderless ? "Use normal preview" : "Use borderless preview");
    }
}
