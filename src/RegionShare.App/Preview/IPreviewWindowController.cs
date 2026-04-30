namespace RegionShare.App.Preview;

public interface IPreviewWindowController
{
    event EventHandler? PreviewModeChanged;

    PreviewWindowMode Mode { get; }

    void SetMode(PreviewWindowMode mode);
}
