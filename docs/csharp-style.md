# C# Style

## Principles
- One public class, enum, interface, or record per file.
- Keep responsibilities narrow.
- Prefer constructor injection for services.
- Avoid static state unless wrapping platform APIs.
- Keep the app local-only; do not add telemetry, analytics, networking, or remote crash reporting unless explicitly approved.
- Use nullable reference types.
- Prefer explicit names over abbreviations.
- Keep UI, capture, settings, hotkeys, and DPI logic separated.
- Add or update tests for every task or feature.

## WPF
- Keep windows responsible for presentation and direct user interaction.
- Move reusable logic into services or models.
- Avoid large code-behind files.
- Use dispatching only at UI boundaries.

## Testing
- Run `dotnet test` before considering any task complete.
- Unit test pure services such as DPI conversion, region math, aspect ratio sizing, hotkey state, and settings serialization.
- For platform APIs that are difficult to test directly, isolate API calls behind interfaces and test service behavior around those boundaries.

## Privacy
- Do not persist captured screen content by default.
- Do not log captured screen content or sensitive user activity.
- Treat dependencies that send telemetry or crash data as blocked unless explicitly approved.
