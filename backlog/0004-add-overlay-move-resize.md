# Add Overlay Move And Resize

## Goal
Allow the user to position and resize the capture region.

## Tasks
- Implement dragging to move the overlay.
- Add edge and corner resize handles.
- Enforce minimum size.
- Update size label during move/resize.
- Keep one class per file for overlay state and resize behavior.

## Acceptance Criteria
- Overlay can be moved.
- Overlay can be resized from edges and corners.
- Current dimensions update live.
- Tests cover resize math and boundary handling.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- Overlay can still be moved by dragging the body.
- Transparent hit-test handles were added for every edge and corner.
- Resize handle cursors indicate the active resize direction.
- Resize math is isolated in `OverlayResizeCalculator` instead of WPF code-behind.
- Minimum width and height are enforced by resize calculations.
- `OverlayResizeCalculatorTests` covers edge, corner, and minimum-size behavior.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
