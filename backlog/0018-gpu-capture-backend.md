# GPU Capture Backend

## Goal
Swap the default capture path from CPU-heavy GDI allocation/copy work to a Direct3D-backed capture path suitable for 60 FPS and higher frame-rate options.

## Plan
- Add a Direct3D 11 Desktop Duplication backend behind `IScreenCaptureService`.
- Keep `GdiScreenCaptureService` as a fallback when GPU duplication is unavailable, for example unsupported Windows/session states or adapter/output failures.
- Map physical capture regions to the matching desktop output and crop from the duplicated GPU frame.
- Preserve cursor capture behavior.
- Preserve existing capture failure behavior.
- Preserve latest-frame-only preview delivery.

## Tests
- Backend factory chooses GPU capture when supported.
- Backend factory falls back to GDI when GPU capture is unsupported.
- Output-region mapping selects the correct monitor/output and computes output-relative crop bounds.
- Invalid or non-intersecting output mappings fail cleanly.
- `dotnet test` passes.

## Acceptance Criteria
- App defaults to GPU-backed capture when available.
- Existing GDI capture remains available as fallback.
- 30/60/90/120 FPS settings still work.
- Preview remains Teams-shareable and controls-free.
- `dotnet build` and `dotnet test` pass.
