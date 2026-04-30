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
