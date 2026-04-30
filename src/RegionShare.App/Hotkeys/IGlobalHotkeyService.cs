namespace RegionShare.App.Hotkeys;

using System.Windows;

public interface IGlobalHotkeyService
{
    void RegisterLockToggle(Action callback);

    void RegisterOverlayVisibilityToggle(Action callback);

    void RegisterWindow(Window window);

    bool TryInvoke(GlobalHotkeyAction action);

    void UnregisterAll();
}
