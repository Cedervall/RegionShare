# FancyZones Compatibility

## Goal
Make the overlay region selector work well with Microsoft PowerToys FancyZones, or document limitations if transparent borderless overlays cannot reliably participate.

## Tasks
- Investigate why the borderless transparent overlay does not snap or resize into FancyZones.
- Determine whether FancyZones requires standard resizable window styles.
- Evaluate options:
  - keep current custom overlay resize behavior only
  - add optional standard-window resize mode
  - add native Win32 resize styles while preserving transparency
  - prefer preset sizes as the practical alternative
- Preserve overlay capture exclusion behavior.
- Preserve meeting-shareable preview behavior.
- Document limitations if FancyZones cannot reliably support transparent overlays.

## Acceptance Criteria
- Overlay can be positioned or resized using FancyZones, or limitations are clearly documented.
- Existing custom move/resize behavior still works.
- Overlay remains excluded from capture where supported.
- Tests cover any window-mode or state logic introduced.
- `dotnet test` passes.

## Status
Completed with the Snap Region helper flow.

## Verification
- The transparent Capture Window keeps its custom move/resize behavior.
- `Snap Region` opens a temporary standard resizable setup window so Windows Snap and compatible zone tools can size the capture region.
- Applying setup bounds updates the capture region and returns to the transparent Capture Window workflow.
- Overlay capture exclusion behavior is preserved.
- Setup-window bounds behavior is covered by tests.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
