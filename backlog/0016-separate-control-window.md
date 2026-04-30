# Separate Control Window From Preview

## Goal
Keep the Teams-shared preview window clean by moving all controls into a separate control/settings window.

## Tasks
- Add `ControlWindow` for capture and overlay controls.
- Move start/stop capture out of `PreviewWindow`.
- Move overlay lock/unlock and show/hide controls out of `PreviewWindow`.
- Keep `PreviewWindow` content-only in both normal and borderless modes.
- Add a `Preview borderless` toggle in `ControlWindow`.
- Apply borderless mode immediately to `PreviewWindow`.
- Closing `PreviewWindow` exits the whole app and closes other windows.
- Closing or hiding `OverlayWindow` keeps capture running from the last selected region.
- Decide whether closing `ControlWindow` hides it or exits only through explicit `Exit` control.

## Acceptance Criteria
- Preview window contains only captured content and no control buttons/status text.
- Control window owns capture, overlay, and preview-mode controls.
- Borderless preview can be toggled from the control window.
- Borderless preview remains shareable in Teams as a normal application window.
- Preview close stops capture, disposes capture resources, closes other windows, and shuts down the app.
- Overlay close/hide does not stop capture.
- Tests cover preview window mode state, control label state, and app lifecycle/controller behavior where practical.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- Added `ControlWindow` for capture, overlay visibility/lock, preview borderless toggle, and exit controls.
- Removed controls from `PreviewWindow`; preview now contains only the capture viewport.
- Added `PreviewWindowController` and `PreviewWindowModeState` for normal/borderless mode switching.
- Closing `PreviewWindow` disposes capture resources and shuts down the app.
- Closing `ControlWindow` hides the control window unless the app is shutting down.
- Tests cover control labels, preview mode state, and preview mode controller events.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
