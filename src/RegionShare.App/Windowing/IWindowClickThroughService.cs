using System.Windows;

namespace RegionShare.App.Windowing;

public interface IWindowClickThroughService
{
    void SetClickThrough(Window window, bool isClickThrough);
}
