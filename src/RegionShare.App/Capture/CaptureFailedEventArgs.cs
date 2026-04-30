namespace RegionShare.App.Capture;

public sealed class CaptureFailedEventArgs : EventArgs
{
    public CaptureFailedEventArgs(Exception exception)
    {
        Exception = exception;
    }

    public Exception Exception { get; }
}
