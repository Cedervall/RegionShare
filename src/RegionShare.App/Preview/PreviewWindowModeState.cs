using System.Windows;

namespace RegionShare.App.Preview;

public sealed record PreviewWindowModeState(WindowStyle WindowStyle, ResizeMode ResizeMode, Thickness ContentMargin)
{
    public static PreviewWindowModeState FromMode(PreviewWindowMode mode)
    {
        return mode switch
        {
            PreviewWindowMode.Borderless => new PreviewWindowModeState(WindowStyle.None, ResizeMode.CanResize, new Thickness(0)),
            _ => new PreviewWindowModeState(WindowStyle.SingleBorderWindow, ResizeMode.CanResize, new Thickness(0))
        };
    }
}
