# Add Lock Mode

## Goal
Prevent accidental movement or resizing of the capture region.

## Tasks
- Add lock state to overlay service.
- Disable move and resize when locked.
- Change border style/color when locked.
- Add UI toggle.
- Add right-click lock/unlock menu if straightforward.

## Acceptance Criteria
- Locked overlay cannot move.
- Locked overlay cannot resize.
- Capture region remains fixed.
- Locked state is visually obvious.
- Tests cover lock state transitions and locked interaction guards.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- Overlay includes a lock/unlock button.
- Overlay right-click menu can lock or unlock the region.
- Locked mode disables resize handles and blocks move/resize logic.
- Border and status text change when locked.
- Lock interaction rules are isolated in `OverlayInteractionGuard`.
- Lock visual mapping is isolated in `OverlayLockVisualState`.
- Tests cover lock state transitions, move/resize guards, and visual state mapping.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
