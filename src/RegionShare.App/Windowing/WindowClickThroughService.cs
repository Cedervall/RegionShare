using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace RegionShare.App.Windowing;

public sealed class WindowClickThroughService : IWindowClickThroughService
{
    private const int ExtendedStyleIndex = -20;

    public void SetClickThrough(Window window, bool isClickThrough)
    {
        ArgumentNullException.ThrowIfNull(window);

        var handle = new WindowInteropHelper(window).Handle;
        if (handle == IntPtr.Zero)
        {
            return;
        }

        var currentStyle = GetWindowLong(handle, ExtendedStyleIndex);
        var newStyle = WindowExtendedStyleCalculator.SetClickThrough(currentStyle, isClickThrough);
        SetWindowLong(handle, ExtendedStyleIndex, newStyle);
    }

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hwnd, int index);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hwnd, int index, int newLong);
}
