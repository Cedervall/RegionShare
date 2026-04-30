# Add Preview Window

## Goal
Create the Teams-shareable preview window.

## Tasks
- Render captured frames into preview.
- Preserve aspect ratio.
- Scale content to fit window.
- Add start/stop capture control.
- Keep preview as a normal application window.

## Acceptance Criteria
- Preview window is visible in Teams as a normal app window.
- Preview scales content without distortion.
- Preview can start and stop capture.
- Tests cover aspect-ratio fit calculations and capture lifecycle logic.
- `dotnet test` passes.
