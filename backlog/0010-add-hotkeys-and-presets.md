# Add Hotkeys And Presets

## Goal
Improve control speed for common meeting workflows.

## Tasks
- Implement global hotkey service.
- Implement `Ctrl + Alt + L` for lock toggle.
- Add overlay show/hide hotkey.
- Add presets: 1280x720, 1600x900, 1920x1080.
- Add aspect ratio modes: Free, 16:9, 16:10, 4:3.

## Acceptance Criteria
- Lock toggles via hotkey.
- Presets resize overlay correctly.
- Aspect ratio mode constrains resize behavior.
- Tests cover preset values, aspect ratio math, and hotkey callback registration.
- `dotnet test` passes.
