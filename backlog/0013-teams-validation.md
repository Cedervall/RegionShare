# Meeting Software Validation

## Goal
Validate the core user workflow with online meeting software.

## Tasks
- Confirm Region Share Window appears in meeting software window sharing pickers.
- Confirm webcam still works normally.
- Confirm viewers see selected region only.
- Confirm overlay is excluded.
- Record any limitations.

## Acceptance Criteria
- Meeting software can share the Region Share Window.
- Remote viewers see multiple apps inside selected region.
- Overlay is not visible to viewers.
- Manual validation notes are captured.
- `dotnet test` passes before validation build is considered acceptable.

## Status
Completed after manual validation with Microsoft Teams.

## Verification
- Region Share Window appeared in the Teams window sharing picker.
- Teams could share the Region Share Window.
- Remote viewers saw the selected region only.
- Capture Window overlay was not visible to viewers.
- Webcam continued to work normally.
- Reviewer outcome: `pass`.
