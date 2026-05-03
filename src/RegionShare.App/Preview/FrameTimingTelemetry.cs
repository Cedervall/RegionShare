namespace RegionShare.App.Preview;

public sealed class FrameTimingTelemetry : IFrameTimingTelemetry
{
    public event EventHandler<FrameTimingSample>? TimingUpdated;

    public FrameTimingSample? Current { get; private set; }

    public void Update(FrameTimingSample sample)
    {
        Current = sample;
        TimingUpdated?.Invoke(this, sample);
    }
}
