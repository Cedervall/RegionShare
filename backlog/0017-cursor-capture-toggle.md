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
