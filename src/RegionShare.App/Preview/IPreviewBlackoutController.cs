namespace RegionShare.App.Preview;

public interface IPreviewBlackoutController
{
    event EventHandler? BlackoutRequested;

    void RequestBlackout();
}
