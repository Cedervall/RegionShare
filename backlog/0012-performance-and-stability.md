# Performance And Stability

## Goal
Make the app stable enough for long meetings.

## Tasks
- Target approximately 30 FPS.
- Measure CPU and memory usage.
- Avoid per-frame large allocations.
- Handle monitor disconnect or capture failure.
- Ensure start/stop can be repeated safely.

## Acceptance Criteria
- App remains stable during extended capture.
- CPU usage is acceptable.
- Capture can recover or fail gracefully.
- Tests cover repeated start/stop and failure-path state transitions.
- `dotnet test` passes.
