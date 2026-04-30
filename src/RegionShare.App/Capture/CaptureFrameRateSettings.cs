namespace RegionShare.App.Capture;

public sealed class CaptureFrameRateSettings : ICaptureFrameRateSettings
{
    public int FramesPerSecond { get; set; } = CaptureFrameRateCalculator.DefaultFramesPerSecond;
}
