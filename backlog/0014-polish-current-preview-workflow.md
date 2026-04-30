# Polish Current Preview Workflow

## Goal
Fix usability issues discovered during manual review before continuing deeper platform work.

## Tasks
- Make resize handles visually transparent so the overlay border appears green instead of grey.
- Hide the `Capture preview will appear here` placeholder after the first captured frame is displayed.
- Keep overlay visible and topmost while unlocked.
- Support click-through behavior when the overlay is locked.
- Add a safe unlock/show path outside the overlay, preferably in the preview window and via hotkey.
- Confirm capture can continue using the last selected region if the overlay is hidden or closed.

## Acceptance Criteria
- Overlay border is visibly green when unlocked.
- Resize handles remain usable but do not visually obscure the border.
- Placeholder text is hidden once capture frames are shown.
- Locked overlay does not block clicking apps behind it.
- User can unlock or show the overlay without clicking the overlay itself.
- Capture continues from the last selected region when the overlay is hidden or closed.
- Tests cover visual-state mapping, placeholder visibility state, and click-through/window-mode state where practical.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- Resize handles use a transparent template so they remain usable without obscuring the overlay border.
- Preview placeholder visibility is controlled by `PreviewPlaceholderState` and is hidden after the first captured frame.
- Preview window has lock/unlock and show/hide controls for the overlay.
- Overlay close is treated as hide, allowing capture to continue from the last selected region.
- Locked overlay applies click-through window style through `IWindowClickThroughService`.
- Tests cover placeholder visibility, preview overlay control labels, and click-through extended-style mapping.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
