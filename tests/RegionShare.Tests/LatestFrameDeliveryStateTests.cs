using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class LatestFrameDeliveryStateTests
{
    [Fact]
    public void EnqueueReturnsTrueOnlyForFirstFrameWhileDispatchIsQueued()
    {
        var state = new LatestFrameDeliveryState<string>();

        Assert.True(state.Enqueue("first"));
        Assert.False(state.Enqueue("second"));
    }

    [Fact]
    public void TakeLatestDropsStaleFrames()
    {
        var state = new LatestFrameDeliveryState<string>();

        state.Enqueue("first");
        state.Enqueue("second");

        Assert.Equal("second", state.TakeLatest());
        Assert.Null(state.TakeLatest());
    }

    [Fact]
    public void CompleteDispatchReturnsFalseWhenNoFrameArrivedDuringDispatch()
    {
        var state = new LatestFrameDeliveryState<string>();
        state.Enqueue("first");
        state.TakeLatest();

        Assert.False(state.CompleteDispatchAndShouldQueueAgain());
    }

    [Fact]
    public void CompleteDispatchReturnsTrueWhenFrameArrivedDuringDispatch()
    {
        var state = new LatestFrameDeliveryState<string>();
        state.Enqueue("first");
        state.TakeLatest();
        state.Enqueue("second");

        Assert.True(state.CompleteDispatchAndShouldQueueAgain());
        Assert.False(state.Enqueue("third"));
    }
}
