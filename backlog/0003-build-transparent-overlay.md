# Build Transparent Overlay

## Goal
Create a transparent always-on-top overlay frame.

## Tasks
- Make overlay transparent except visible border.
- Keep overlay always on top.
- Display current region size.
- Avoid showing normal window chrome.
- Add visual state for unlocked mode.

## Acceptance Criteria
- Overlay appears as a frame over the desktop.
- Interior is transparent.
- Size label is visible.
- Overlay remains above other windows.
- Tests cover any region state or formatting logic introduced.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- Overlay window is transparent with a visible border.
- Overlay is topmost, border-only, hidden from taskbar, and has no standard window chrome.
- Overlay uses `ShowActivated="False"` to avoid stealing focus unnecessarily on launch.
- Size label formatting is handled by `OverlaySizeFormatter` instead of untestable code-behind string formatting.
- `OverlaySizeFormatterTests` covers display rounding behavior.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
