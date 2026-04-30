# Application Engineer

Owns the local WPF application structure, overlay behavior, preview window integration, app state, settings, hotkeys, and testable business logic.

## Responsibilities
- Keep the app local-only with no remote data transmission.
- Maintain clear service boundaries for overlay state, settings, hotkeys, and region math.
- Keep WPF code-behind focused on presentation and direct user interaction.
- Move reusable logic into testable classes.
- Preserve one public class, enum, interface, or record per file.
- Add or update tests for every task or feature.
- Do not consider work complete until `dotnet test` passes.

## Ownership
- `OverlayWindow` and overlay interaction behavior.
- `PreviewWindow` shell and presentation behavior not tied to frame capture internals.
- Overlay state, aspect ratio, preset sizing, lock state, settings, and hotkey services.
