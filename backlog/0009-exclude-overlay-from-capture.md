# Exclude Overlay From Capture

## Goal
Ensure the overlay frame is not visible in the captured output.

## Tasks
- Investigate Windows display affinity and capture exclusion APIs.
- Apply exclusion to overlay window if supported.
- Validate capture output while overlay is visible.
- Document fallback behavior if OS limitations exist.

## Acceptance Criteria
- Overlay border and label do not appear in preview.
- Preview shows underlying desktop content only.
- Tests cover exclusion service state or API wrapper behavior where practical.
- `dotnet test` passes.

## Status
Manual validation required before completion.

## Verification
- Overlay window applies `SetWindowDisplayAffinity` through `WindowCaptureExclusionService` when its handle is initialized.
- Tests cover the capture exclusion Win32 boundary for zero handles, expected affinity value, and failure propagation.
- Manual validation steps are documented in `docs/capture-exclusion.md`.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: pending manual validation.
