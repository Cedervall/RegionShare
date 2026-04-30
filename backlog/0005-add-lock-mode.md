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
