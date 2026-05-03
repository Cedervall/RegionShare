using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class FrameTimingTelemetryTests
{
    [Fact]
    public void UpdateStoresCurrentSampleAndPublishesEvent()
    {
        var telemetry = new FrameTimingTelemetry();
        var sample = new FrameTimingSample(5, 16.7);
        FrameTimingSample? publishedSample = null;
        telemetry.TimingUpdated += (_, e) => publishedSample = e;

        telemetry.Update(sample);

        Assert.Equal(sample, telemetry.Current);
        Assert.Equal(sample, publishedSample);
    }
}
