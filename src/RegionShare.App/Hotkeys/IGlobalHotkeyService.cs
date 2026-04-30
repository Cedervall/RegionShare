namespace RegionShare.App.Hotkeys;

public interface IGlobalHotkeyService
{
    void RegisterLockToggle(Action callback);

    void UnregisterAll();
}
