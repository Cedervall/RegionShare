using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Dpi;
using RegionShare.App.Hotkeys;
using RegionShare.App.Overlay;
using RegionShare.App.Preview;
using RegionShare.App.Settings;
using RegionShare.App.Windowing;
using RegionShare.App.Windows;

namespace RegionShare.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var overlayState = new OverlayStateService();
        var settingsService = new UserSettingsService();
        var settings = settingsService.Load();
        var cursorCaptureSettings = new CursorCaptureSettings
        {
            IsCursorCaptureEnabled = settings.IsCursorCaptureEnabled
        };
        var captureFrameRateSettings = new CaptureFrameRateSettings
        {
            FramesPerSecond = settings.CaptureFramesPerSecond
        };
        var captureService = new CaptureServiceManager(cursorCaptureSettings, captureFrameRateSettings, new Direct3DDesktopDuplicationSupport());
        var hotkeyService = new GlobalHotkeyService();
        var previewWindowController = new PreviewWindowController();
        var previewBlackoutController = new PreviewBlackoutController();
        var frameTimingTelemetry = new FrameTimingTelemetry();
        var overlayWindow = new OverlayWindow(overlayState, new DpiService(), new WindowCaptureExclusionService(), new WindowClickThroughService(), frameTimingTelemetry, captureService);
        var previewWindow = new PreviewWindow(captureService, overlayWindow.GetCaptureRegion, previewWindowController, frameTimingTelemetry, previewBlackoutController);
        var controlWindow = new ControlWindow(captureService, overlayWindow.GetCaptureRegion, overlayWindow, previewWindow, previewWindowController, previewBlackoutController, hotkeyService, cursorCaptureSettings, captureFrameRateSettings);

        ApplySettings(settings, overlayWindow, previewWindow, controlWindow, previewWindowController);

        Exit += (_, _) => settingsService.Save(CreateSettings(overlayWindow, previewWindow, controlWindow, previewWindowController, cursorCaptureSettings, captureFrameRateSettings));

        overlayWindow.Show();
        previewWindow.Show();
        controlWindow.Show();
    }

    private static void ApplySettings(UserSettings settings, OverlayWindow overlayWindow, PreviewWindow previewWindow, ControlWindow controlWindow, IPreviewWindowController previewWindowController)
    {
        var workAreas = ScreenWorkAreaProvider.GetActiveWorkAreas();
        var overlayBounds = WindowPlacementCalculator.EnsureVisible(new Rect(settings.OverlayLeft, settings.OverlayTop, settings.OverlayWidth, settings.OverlayHeight), new Size(overlayWindow.MinWidth, overlayWindow.MinHeight), workAreas, new Point(24, 24));
        var previewBounds = WindowPlacementCalculator.EnsureVisible(new Rect(settings.PreviewLeft, settings.PreviewTop, settings.PreviewWidth, settings.PreviewHeight), new Size(previewWindow.MinWidth, previewWindow.MinHeight), workAreas, new Point(80, 80));
        var controlBounds = WindowPlacementCalculator.EnsureVisible(new Rect(settings.ControlLeft, settings.ControlTop, settings.ControlWidth, settings.ControlHeight), new Size(controlWindow.MinWidth, 260), workAreas, new Point(136, 136));

        overlayWindow.SetRegionBounds(overlayBounds);
        overlayWindow.SetAspectRatioMode(settings.AspectRatioMode);
        overlayWindow.SetStatusVisibility(settings.IsOverlayStatusVisible ?? true);
        overlayWindow.SetLatencyVisibility(settings.IsOverlayLatencyVisible ?? true);
        if (settings.IsLocked)
        {
            overlayWindow.ToggleLock();
        }

        previewWindow.Left = previewBounds.Left;
        previewWindow.Top = previewBounds.Top;
        previewWindow.Width = previewBounds.Width;
        previewWindow.Height = previewBounds.Height;
        previewWindowController.SetMode(settings.IsPreviewBorderless ? PreviewWindowMode.Borderless : PreviewWindowMode.Normal);

        controlWindow.Left = controlBounds.Left;
        controlWindow.Top = controlBounds.Top;
        controlWindow.Width = controlBounds.Width;
    }

    private static UserSettings CreateSettings(OverlayWindow overlayWindow, PreviewWindow previewWindow, ControlWindow controlWindow, IPreviewWindowController previewWindowController, ICursorCaptureSettings cursorCaptureSettings, ICaptureFrameRateSettings captureFrameRateSettings)
    {
        return new UserSettings(
            overlayWindow.Left,
            overlayWindow.Top,
            overlayWindow.Width,
            overlayWindow.Height,
            overlayWindow.IsOverlayVisible,
            overlayWindow.IsLocked,
            overlayWindow.AspectRatioMode,
            previewWindow.Left,
            previewWindow.Top,
            previewWindow.Width,
            previewWindow.Height,
            previewWindowController.Mode == PreviewWindowMode.Borderless,
            controlWindow.Left,
            controlWindow.Top,
            controlWindow.Width,
            controlWindow.Height,
            cursorCaptureSettings.IsCursorCaptureEnabled,
            captureFrameRateSettings.FramesPerSecond,
            overlayWindow.IsStatusVisible,
            overlayWindow.IsLatencyVisible);
    }
}

