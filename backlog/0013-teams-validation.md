# Teams Validation

## Goal
Validate the core user workflow with Microsoft Teams.

## Tasks
- Confirm preview window appears in Teams window sharing picker.
- Confirm webcam still works normally.
- Confirm viewers see selected region only.
- Confirm overlay is excluded.
- Record any limitations.

## Acceptance Criteria
- Teams can share `Region Share Preview`.
- Remote viewers see multiple apps inside selected region.
- Overlay is not visible to viewers.
- Manual validation notes are captured.
- `dotnet test` passes before validation build is considered acceptable.
