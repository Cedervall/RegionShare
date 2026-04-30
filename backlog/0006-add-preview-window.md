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

## Status
Completed in this ticket batch.

## Verification
- Preview window remains a normal desktop window titled `Region Share Preview`.
- Preview window includes start/stop capture controls and visible capture status.
- Capture toggle behavior is isolated in `PreviewCaptureController`.
- Aspect-ratio fit math is isolated in `PreviewFitCalculator`.
- Tests cover start/stop lifecycle behavior and aspect-ratio fit calculations.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
