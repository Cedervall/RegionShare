# Add Hotkeys And Presets

## Goal
Improve control speed for common meeting workflows.

## Tasks
- Implement global hotkey service.
- Implement `Ctrl + Alt + L` for lock toggle.
- Add overlay show/hide hotkey.
- Keep lock/unlock and show/hide overlay controls in the control window, so the Teams-shared preview remains content-only.
- Make locked overlay click-through while keeping the capture region fixed.
- Add presets: 1280x720, 1600x900, 1920x1080.
- Add aspect ratio modes: Free, 16:9, 16:10, 4:3.

## Acceptance Criteria
- Lock toggles via hotkey.
- Lock toggles from the control window.
- Hidden overlay can be shown again from the control window or hotkey.
- Locked overlay allows interacting with windows behind it.
- Presets resize overlay correctly.
- Aspect ratio mode constrains resize behavior.
- Tests cover preset values, aspect ratio math, click-through mode mapping, overlay visibility state, and hotkey callback registration.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- Added global hotkeys: `Ctrl + Alt + L` toggles overlay lock and `Ctrl + Alt + O` toggles overlay visibility.
- Added preset buttons for 1280 x 720, 1600 x 900, and 1920 x 1080 in the control window.
- Added aspect ratio selection for Free, 16:9, 16:10, and 4:3.
- Overlay resize math can constrain to the selected aspect ratio mode.
- Control window displays hotkey hints.
- Tests cover preset values, aspect-ratio calculations, aspect-ratio resize behavior, and hotkey callback registration/removal.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
