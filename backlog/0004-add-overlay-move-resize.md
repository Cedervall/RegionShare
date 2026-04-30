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
