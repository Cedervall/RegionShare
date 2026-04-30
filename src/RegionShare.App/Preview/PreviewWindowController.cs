namespace RegionShare.App.Preview;

public sealed class PreviewWindowController : IPreviewWindowController
{
    public event EventHandler? PreviewModeChanged;

    public PreviewWindowMode Mode { get; private set; } = PreviewWindowMode.Normal;

    public void SetMode(PreviewWindowMode mode)
    {
        if (Mode == mode)
        {
            return;
        }

        Mode = mode;
        PreviewModeChanged?.Invoke(this, EventArgs.Empty);
    }
}
