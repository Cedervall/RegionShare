# Implement Region Capture

## Goal
Capture the selected rectangular screen region at approximately 30 FPS.

## Tasks
- Implement `IScreenCaptureService`.
- Prefer Windows Graphics Capture.
- Capture physical pixel region derived from overlay bounds.
- Stream frames to preview renderer.
- Avoid unnecessary allocations during frame updates.

## Acceptance Criteria
- Preview displays live content from selected region.
- Capture updates smoothly.
- Multiple apps inside region are visible.
- Tests cover capture service state and invalid region handling.
- `dotnet test` passes.

## Status
Completed with an interim local GDI capture backend.

## Verification
- `IScreenCaptureService` now publishes captured frame events.
- `GdiScreenCaptureService` captures the selected rectangular screen region on a ~33 ms timer.
- Preview window renders captured frames into its image area.
- Overlay capture exclusion is applied when the overlay window handle is initialized.
- Overlay bounds are converted from WPF logical units to physical pixels before capture.
- Preview window unsubscribes from frame events and disposes disposable capture services when closed.
- Capture remains local-only and does not persist or transmit screen contents.
- Windows Graphics Capture remains the target replacement backend behind `IScreenCaptureService`.
- Tests cover invalid region handling, capture start/stop/dispose state, and the capture exclusion service boundary.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
