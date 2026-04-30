namespace RegionShare.App.Hotkeys;

using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

public sealed class GlobalHotkeyService : IGlobalHotkeyService
{
    private const int LockHotkeyId = 9001;
    private const int OverlayVisibilityHotkeyId = 9002;
    private const int WindowsHotkeyMessage = 0x0312;
    private const uint AltModifier = 0x0001;
    private const uint ControlModifier = 0x0002;
    private Action? _lockToggleCallback;
    private Action? _overlayVisibilityToggleCallback;
    private HwndSource? _source;
    private IntPtr _windowHandle;

    public void RegisterLockToggle(Action callback)
    {
        _lockToggleCallback = callback;
    }

    public void RegisterOverlayVisibilityToggle(Action callback)
    {
        _overlayVisibilityToggleCallback = callback;
    }

    public void RegisterWindow(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        _windowHandle = new WindowInteropHelper(window).Handle;
        if (_windowHandle == IntPtr.Zero)
        {
            return;
        }

        _source = HwndSource.FromHwnd(_windowHandle);
        _source?.AddHook(ProcessWindowMessage);

        RegisterHotKey(_windowHandle, LockHotkeyId, AltModifier | ControlModifier, (uint)KeyInterop.VirtualKeyFromKey(Key.L));
        RegisterHotKey(_windowHandle, OverlayVisibilityHotkeyId, AltModifier | ControlModifier, (uint)KeyInterop.VirtualKeyFromKey(Key.O));
    }

    public bool TryInvoke(GlobalHotkeyAction action)
    {
        var callback = action switch
        {
            GlobalHotkeyAction.ToggleLock => _lockToggleCallback,
            GlobalHotkeyAction.ToggleOverlayVisibility => _overlayVisibilityToggleCallback,
            _ => null
        };

        if (callback is null)
        {
            return false;
        }

        callback();
        return true;
    }

    public void UnregisterAll()
    {
        if (_windowHandle != IntPtr.Zero)
        {
            UnregisterHotKey(_windowHandle, LockHotkeyId);
            UnregisterHotKey(_windowHandle, OverlayVisibilityHotkeyId);
        }

        _source?.RemoveHook(ProcessWindowMessage);
        _source = null;
        _windowHandle = IntPtr.Zero;
        _lockToggleCallback = null;
        _overlayVisibilityToggleCallback = null;
    }

    private IntPtr ProcessWindowMessage(IntPtr hwnd, int message, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (message != WindowsHotkeyMessage)
        {
            return IntPtr.Zero;
        }

        handled = (int)wParam switch
        {
            LockHotkeyId => TryInvoke(GlobalHotkeyAction.ToggleLock),
            OverlayVisibilityHotkeyId => TryInvoke(GlobalHotkeyAction.ToggleOverlayVisibility),
            _ => false
        };

        return IntPtr.Zero;
    }

    [DllImport("user32.dll")]
    private static extern bool RegisterHotKey(IntPtr hwnd, int id, uint modifiers, uint virtualKey);

    [DllImport("user32.dll")]
    private static extern bool UnregisterHotKey(IntPtr hwnd, int id);
}
