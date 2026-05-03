namespace RegionShare.App.Preview;

public sealed class PreviewBlackoutController : IPreviewBlackoutController
{
    public event EventHandler? BlackoutRequested;

    public void RequestBlackout()
    {
        BlackoutRequested?.Invoke(this, EventArgs.Empty);
    }
}
