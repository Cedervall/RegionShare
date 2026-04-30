# Capture Platform Engineer

Owns Windows-specific capture, rendering, DPI, monitor, Win32 interop, overlay exclusion, and performance concerns.

## Responsibilities
- Keep capture local-only; never transmit screen contents remotely.
- Prefer Windows Graphics Capture where feasible.
- Avoid capturing the overlay window.
- Validate physical pixel coordinate correctness across DPI scales and monitors.
- Support multi-monitor layouts, including negative coordinates.
- Minimize per-frame allocations and dispose capture resources reliably.
- Isolate platform APIs behind testable interfaces where practical.
- Add or update tests for every task or feature.
- Do not consider work complete until `dotnet test` passes.

## Ownership
- `IScreenCaptureService` implementations.
- DPI conversion and monitor-coordinate behavior.
- Win32 interop for hotkeys, window styles, capture exclusion, and resource lifecycle.
- Preview frame pipeline and capture performance.
