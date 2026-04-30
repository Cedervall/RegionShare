# Architecture

## Window Model
The app has two top-level WPF windows.

`OverlayWindow` defines the capture region. It is transparent except for its border and controls.

`PreviewWindow` is a normal desktop window titled `Region Share Preview`. This is the window shared in Microsoft Teams.

## Services
- `IOverlayStateService` owns overlay lock and aspect-ratio state.
- `IScreenCaptureService` owns capture lifecycle.
- `IDpiService` converts WPF logical coordinates to physical screen pixels.
- `IGlobalHotkeyService` owns process-level hotkey registration.
- `IUserSettingsService` owns persistence of user preferences.

## Privacy Boundary
Region Share is a local-only desktop app. Screen contents, telemetry, settings, logs, crash dumps, and user activity must not be transmitted to any remote service unless the user explicitly adds and enables such a feature later.

Captured screen content should not be persisted by default. If logging is added, logs must not include captured screen content or sensitive user activity.

## Testing Requirement
Every backlog task and feature must add or update tests where practical. `dotnet test` must pass before the task is accepted.

## Workflow
All work follows the ticket flow in `docs/workflow.md`. Work may only be committed after the reviewer returns `pass`.

## Capture Direction
The target implementation is Windows Graphics Capture. If MVP delivery requires a simpler interim capture implementation, it must remain behind `IScreenCaptureService` so the window and overlay code do not change when the capture backend is replaced.
