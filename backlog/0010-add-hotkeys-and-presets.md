# Add Hotkeys And Presets

## Goal
Improve control speed for common meeting workflows.

## Tasks
- Implement global hotkey service.
- Implement `Ctrl + Alt + L` for lock toggle.
- Add overlay show/hide hotkey.
- Add preview-window controls for lock/unlock and show/hide overlay, so the user can recover when the overlay is hidden or click-through.
- Make locked overlay click-through while keeping the capture region fixed.
- Add presets: 1280x720, 1600x900, 1920x1080.
- Add aspect ratio modes: Free, 16:9, 16:10, 4:3.

## Acceptance Criteria
- Lock toggles via hotkey.
- Lock toggles from the preview window.
- Hidden overlay can be shown again from the preview window or hotkey.
- Locked overlay allows interacting with windows behind it.
- Presets resize overlay correctly.
- Aspect ratio mode constrains resize behavior.
- Tests cover preset values, aspect ratio math, click-through mode mapping, overlay visibility state, and hotkey callback registration.
- `dotnet test` passes.
