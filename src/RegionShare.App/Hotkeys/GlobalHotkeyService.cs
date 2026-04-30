namespace RegionShare.App.Hotkeys;

public sealed class GlobalHotkeyService : IGlobalHotkeyService
{
    private Action? _lockToggleCallback;

    public void RegisterLockToggle(Action callback)
    {
        _lockToggleCallback = callback;
    }

    public void UnregisterAll()
    {
        _lockToggleCallback = null;
    }
}
