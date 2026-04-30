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
