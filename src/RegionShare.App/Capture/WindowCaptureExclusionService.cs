using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace RegionShare.App.Capture;

public sealed class WindowCaptureExclusionService : IWindowCaptureExclusionService
{
    public const uint ExcludeFromCaptureAffinity = 0x00000011;
    private readonly Func<IntPtr, uint, bool> _setWindowDisplayAffinity;

    public WindowCaptureExclusionService()
        : this(SetWindowDisplayAffinity)
    {
    }

    public WindowCaptureExclusionService(Func<IntPtr, uint, bool> setWindowDisplayAffinity)
    {
        _setWindowDisplayAffinity = setWindowDisplayAffinity;
    }

    public bool ExcludeFromCapture(Window window)
    {
        ArgumentNullException.ThrowIfNull(window);

        var handle = new WindowInteropHelper(window).Handle;
        return ExcludeHandleFromCapture(handle);
    }

    public bool ExcludeHandleFromCapture(IntPtr handle)
    {
        return handle != IntPtr.Zero && _setWindowDisplayAffinity(handle, ExcludeFromCaptureAffinity);
    }

    [DllImport("user32.dll")]
    private static extern bool SetWindowDisplayAffinity(IntPtr hwnd, uint affinity);
}
