namespace RegionShare.App.Preview;

public interface IFrameTimingTelemetry
{
    event EventHandler<FrameTimingSample>? TimingUpdated;

    FrameTimingSample? Current { get; }

    void Update(FrameTimingSample sample);
}
