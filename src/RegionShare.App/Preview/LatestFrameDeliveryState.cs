namespace RegionShare.App.Preview;

public sealed class LatestFrameDeliveryState<TFrame>
    where TFrame : class
{
    private TFrame? _latestFrame;
    private int _isDispatchQueued;

    public bool Enqueue(TFrame frame)
    {
        Interlocked.Exchange(ref _latestFrame, frame);
        return Interlocked.Exchange(ref _isDispatchQueued, 1) == 0;
    }

    public TFrame? TakeLatest()
    {
        return Interlocked.Exchange(ref _latestFrame, null);
    }

    public bool CompleteDispatchAndShouldQueueAgain()
    {
        Interlocked.Exchange(ref _isDispatchQueued, 0);
        if (Volatile.Read(ref _latestFrame) is null)
        {
            return false;
        }

        return Interlocked.Exchange(ref _isDispatchQueued, 1) == 0;
    }
}
