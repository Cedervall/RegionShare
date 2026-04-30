namespace RegionShare.App.Preview;

using RegionShare.App.Capture;

public static class CaptureFrameRateToggleController
{
    public static CaptureFrameRateControlState Apply(int framesPerSecond, ICaptureFrameRateSettings settings)
    {
        var sanitizedFramesPerSecond = CaptureFrameRateCalculator.Sanitize(framesPerSecond);
        settings.FramesPerSecond = sanitizedFramesPerSecond;
        return CaptureFrameRateControlState.FromFramesPerSecond(sanitizedFramesPerSecond);
    }
}
