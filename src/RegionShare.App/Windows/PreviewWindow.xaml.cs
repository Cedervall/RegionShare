using System.Windows;
using RegionShare.App.Capture;
using RegionShare.App.Preview;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace RegionShare.App.Windows;

public partial class PreviewWindow : Window
{
    private readonly IScreenCaptureService _captureService;
    private readonly IPreviewWindowController _previewWindowController;
    private readonly IPreviewBlackoutController _previewBlackoutController;
    private readonly Func<CaptureRegion> _regionProvider;
    private readonly IFrameTimingTelemetry _frameTimingTelemetry;
    private readonly LatestFrameDeliveryState<CapturedFrameEventArgs> _latestFrameDelivery = new();
    private long? _previousPresentedTimestamp;

    public PreviewWindow(IScreenCaptureService captureService, Func<CaptureRegion> regionProvider, IPreviewWindowController previewWindowController, IFrameTimingTelemetry frameTimingTelemetry, IPreviewBlackoutController previewBlackoutController)
    {
        _captureService = captureService;
        _previewWindowController = previewWindowController;
        _previewBlackoutController = previewBlackoutController;
        _regionProvider = regionProvider;
        _frameTimingTelemetry = frameTimingTelemetry;
        captureService.FrameCaptured += CaptureService_FrameCaptured;
        previewWindowController.PreviewModeChanged += PreviewWindowController_PreviewModeChanged;
        previewBlackoutController.BlackoutRequested += PreviewBlackoutController_BlackoutRequested;
        InitializeComponent();
        UpdatePreviewMode();
        UpdatePreviewLayout();
    }

    private void PreviewViewport_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdatePreviewLayout();
    }

    protected override void OnClosed(EventArgs e)
    {
        _captureService.FrameCaptured -= CaptureService_FrameCaptured;
        _previewWindowController.PreviewModeChanged -= PreviewWindowController_PreviewModeChanged;
        _previewBlackoutController.BlackoutRequested -= PreviewBlackoutController_BlackoutRequested;
        if (_captureService is IDisposable disposableCaptureService)
        {
            disposableCaptureService.Dispose();
        }

        Application.Current.Shutdown();
        base.OnClosed(e);
    }

    private void CaptureService_FrameCaptured(object? sender, CapturedFrameEventArgs e)
    {
        if (!_captureService.IsCapturing)
        {
            return;
        }

        if (!Dispatcher.CheckAccess())
        {
            if (_latestFrameDelivery.Enqueue(e))
            {
                Dispatcher.BeginInvoke(ProcessLatestFrame);
            }

            return;
        }

        ApplyFrame(e);
    }

    private void PreviewBlackoutController_BlackoutRequested(object? sender, EventArgs e)
    {
        if (!Dispatcher.CheckAccess())
        {
            Dispatcher.BeginInvoke(() => PreviewBlackoutController_BlackoutRequested(sender, e));
            return;
        }

        ApplyBlackout();
    }

    private void ProcessLatestFrame()
    {
        var frameEvent = _latestFrameDelivery.TakeLatest();
        if (frameEvent is not null)
        {
            ApplyFrame(frameEvent);
        }

        if (_latestFrameDelivery.CompleteDispatchAndShouldQueueAgain())
        {
            Dispatcher.BeginInvoke(ProcessLatestFrame);
        }
    }

    private void ApplyFrame(CapturedFrameEventArgs frameEvent)
    {
        if (!_captureService.IsCapturing)
        {
            return;
        }

        PreviewImage.Source = frameEvent.Frame;
        CapturePlaceholderText.Visibility = PreviewPlaceholderState.GetPlaceholderVisibility(true);
        UpdateFrameTiming(frameEvent.CapturedTimestamp);
    }

    private void ApplyBlackout()
    {
        var state = PreviewBlackoutState.Blackout;
        PreviewImage.Source = (BitmapSource?)state.PreviewSource;
        CapturePlaceholderText.Visibility = state.PlaceholderVisibility;
        _previousPresentedTimestamp = null;
    }

    private void UpdateFrameTiming(long capturedTimestamp)
    {
        var presentedTimestamp = Stopwatch.GetTimestamp();
        var sample = FrameTimingCalculator.Calculate(capturedTimestamp, presentedTimestamp, _previousPresentedTimestamp, Stopwatch.Frequency);
        _previousPresentedTimestamp = presentedTimestamp;
        _frameTimingTelemetry.Update(sample);
    }

    private void PreviewWindowController_PreviewModeChanged(object? sender, EventArgs e)
    {
        UpdatePreviewMode();
    }

    private void UpdatePreviewMode()
    {
        var state = PreviewWindowModeState.FromMode(_previewWindowController.Mode);
        WindowStyle = state.WindowStyle;
        ResizeMode = state.ResizeMode;
        PreviewViewport.Margin = state.ContentMargin;
    }

    private void UpdatePreviewLayout()
    {
        if (PreviewViewport is null || PreviewPlaceholder is null)
        {
            return;
        }

        var region = _regionProvider();
        var fittedSize = PreviewFitCalculator.Fit(
            new Size(region.Width, region.Height),
            new Size(PreviewViewport.ActualWidth, PreviewViewport.ActualHeight));

        if (fittedSize.IsEmpty)
        {
            PreviewPlaceholder.Width = double.NaN;
            PreviewPlaceholder.Height = double.NaN;
            return;
        }

        PreviewPlaceholder.Width = fittedSize.Width;
        PreviewPlaceholder.Height = fittedSize.Height;
    }
}
