namespace RegionShare.App.Capture;

public static class GdiCaptureResourcePlan
{
    public static bool ShouldRecreateBitmap(int currentWidth, int currentHeight, CaptureRegion nextRegion)
    {
        return currentWidth != nextRegion.Width || currentHeight != nextRegion.Height;
    }
}
