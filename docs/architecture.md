# Architecture

## Window Model

RegionShare has three user-facing windows.

- **Capture Window** (`OverlayWindow`): transparent region selector used to define the screen area being captured. It owns region bounds, lock/click-through behavior, show/hide behavior, optional size/status display, optional latency display, presets, and aspect-ratio mode.
- **Control Window** (`ControlWindow`): command and settings surface. It starts/stops capture, controls the capture window, opens Snap Region setup, manages presets/aspect ratios, FPS, cursor capture, preview mode, and overlay display options.
- **Region Share Window** (`PreviewWindow`): Teams-shareable output window. It presents captured frames and resets to the default placeholder when capture stops or fails.

`RegionSetupWindow` is a temporary helper opened by `Snap Region`. It is a normal resizable WPF window so Windows Snap and FancyZones can size the capture region. Applying it copies those bounds back to the Capture Window and then closes the helper.

## Capture Model

`IScreenCaptureService` owns capture lifecycle and frame events.

The app currently selects between:

- Direct3D Desktop Duplication for the default high-performance capture path when supported.
- GDI capture fallback when GPU capture is unavailable or cursor capture is enabled.

Capture services publish `CapturedFrameEventArgs` containing the frame and capture timestamp. `PreviewWindow` presents only the latest queued frame so UI rendering does not backlog.

## Services

- `IOverlayStateService` owns overlay lock and aspect-ratio state.
- `IOverlayController` exposes Capture Window actions to the Control Window.
- `IScreenCaptureService` owns capture lifecycle.
- `IDpiService` converts WPF logical coordinates to physical screen pixels.
- `IGlobalHotkeyService` owns process-level hotkey registration.
- `IUserSettingsService` owns persistence of local user preferences.
- `IPreviewWindowController` owns preview window mode.
- `IPreviewBlackoutController` requests preview reset when capture stops or fails.
- `IFrameTimingTelemetry` shares frame timing samples from the Region Share Window to the Capture Window.

## Settings

User settings are persisted locally under `%LOCALAPPDATA%\RegionShare\settings.json`.

Settings include window positions/sizes, capture window lock state, aspect ratio, preview mode, cursor capture, FPS, and overlay status/latency visibility. The Control Window restores width and position, but its height auto-sizes to the current content.

Cursor capture uses the GDI backend. If cursor capture is toggled while capture is running, the current capture continues unchanged and the Control Window asks the user to restart capture for the backend change to apply.

Capture running state, screen contents, cursor positions, and user activity are not persisted.

## Privacy Boundary

RegionShare is a local-only desktop app. Screen contents, telemetry, settings, logs, crash dumps, and user activity must not be transmitted to any remote service unless the user explicitly adds and enables such a feature later.

Captured screen content should not be persisted by default. If logging is added, logs must not include captured screen content or sensitive user activity.

## Testing Requirement

Every backlog task and feature must add or update tests where practical. `dotnet test` must pass before the task is accepted.

## Workflow

All work follows the ticket flow in `docs/workflow.md`. Work may only be committed after the reviewer returns `pass`.
