# Handle DPI And Multi-Monitor

## Goal
Correctly map WPF logical coordinates to physical screen pixels.

## Tasks
- Implement `IDpiService`.
- Convert overlay bounds to physical pixels.
- Support negative monitor coordinates.
- Validate mixed-DPI monitor scenarios.
- Add unit tests for coordinate conversion.

## Acceptance Criteria
- Captured region matches overlay bounds.
- Works when overlay is on secondary monitors.
- Works with common DPI scaling values.
- Tests cover logical-to-physical conversion, rounding, and negative coordinates.
- `dotnet test` passes.

## Status
Completed in this ticket batch.

## Verification
- `DpiService` validates DPI scale inputs before converting coordinates.
- Logical overlay bounds are converted to physical pixels with midpoint rounding away from zero.
- Negative monitor coordinates are preserved during conversion.
- Tests cover common DPI scales, negative coordinates, midpoint rounding, and invalid scale handling.
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
