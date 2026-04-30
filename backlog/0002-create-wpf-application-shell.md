# Create WPF Application Shell

## Goal
Create the initial Windows desktop application shell.

## Tasks
- Add WPF app project targeting .NET 8 for Windows.
- Add `OverlayWindow` and `PreviewWindow`.
- Ensure app starts both windows.
- Set preview title to `Region Share Preview`.
- Add minimal service/model structure for capture, overlay, DPI, hotkeys, and settings.

## Acceptance Criteria
- App launches successfully.
- Overlay window appears.
- Preview window appears as a normal shareable desktop window.
- `dotnet build` passes.
- `dotnet test` passes.
