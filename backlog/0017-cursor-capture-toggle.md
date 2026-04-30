# Cursor Capture Toggle

## Goal
Allow the preview to optionally include the mouse pointer inside the selected capture region.

## Tasks
- Add a cursor capture setting/toggle in the control window.
- Capture cursor visibility, screen position, and cursor shape using Windows cursor APIs.
- Draw the cursor onto captured frames when enabled and inside the selected region.
- Do not draw the cursor when disabled or outside the selected region.
- Keep cursor capture local-only.
- Avoid persisting cursor positions or user activity.

## Acceptance Criteria
- Preview shows the mouse pointer when it is inside the selected region and cursor capture is enabled.
- Preview does not show the mouse pointer when it is outside the selected region.
- Cursor capture can be toggled on/off.
- Cursor positions are not persisted or logged.
- Tests cover cursor-in-region coordinate math and toggle behavior.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- Control window includes a cursor capture toggle.
- GDI capture draws the current cursor into frames only when cursor capture is enabled and the cursor is inside the selected region.
- Cursor positions are only used per-frame and are not logged or persisted.
- Cursor capture enabled/disabled preference is persisted as a boolean setting.
- Tests cover cursor coordinate mapping and cursor capture toggle state.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
