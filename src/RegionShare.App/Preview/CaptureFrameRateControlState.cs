namespace RegionShare.App.Preview;

using RegionShare.App.Capture;

public sealed record CaptureFrameRateControlState(int FramesPerSecond, string Label)
{
    public static CaptureFrameRateControlState FromFramesPerSecond(int framesPerSecond)
    {
        var sanitizedFramesPerSecond = CaptureFrameRateCalculator.Sanitize(framesPerSecond);
        return new CaptureFrameRateControlState(sanitizedFramesPerSecond, $"Capture FPS: {sanitizedFramesPerSecond}");
    }
}
