# Reviewer

Review for correctness, maintainability, Windows desktop edge cases, and meeting-readiness.

## Checklist
- App remains local-only and introduces no remote transmission of screen contents, telemetry, settings, logs, crash dumps, or user activity.
- No telemetry, analytics, or networking packages are added unless explicitly approved.
- Captured screen content is not persisted unless explicitly required.
- Logs must not include captured screen content or sensitive user activity.
- Overlay exclusion works.
- Overlay is not visible in captured output.
- DPI conversions are correct.
- Mixed-DPI monitor behavior is handled.
- Multi-monitor layouts, including negative coordinates, are handled.
- Lock mode cannot be bypassed by drag or resize.
- Preview is Teams-shareable.
- Capture lifecycle is safe.
- Capture resources are disposed on stop and app shutdown.
- Global hotkeys are unregistered on shutdown.
- Code follows one class per file and clear responsibility boundaries.
- Reusable logic is testable and not hidden in WPF code-behind.
- Every task or feature includes meaningful test coverage where practical.
- `dotnet test` passes before approval.

## Test Quality Gate
The reviewer must verify that tests are meaningful, not merely present.

Reject tests that:
- Only instantiate objects without asserting behavior.
- Only assert that code does not throw unless that is the behavior under test.
- Duplicate the implementation logic instead of checking observable behavior.
- Would still pass if the feature or regression fix were removed.
- Only verify mocks instead of real project logic.
- Ignore important edge cases from the ticket.
- Assert implementation details that do not protect user-visible or architectural behavior.

Prefer tests that:
- Fail when the protected behavior regresses.
- Assert observable behavior and important state transitions.
- Cover happy paths, invalid inputs, and relevant edge cases.
- Test pure logic directly when platform APIs are involved.
- Keep platform API calls behind interfaces and test behavior around those boundaries.
- Verify lifecycle cleanup where relevant, including capture stop, app shutdown, and hotkey unregister behavior.

## Review Outcomes
The reviewer must return one of two outcomes.

`pass`: The work is correct, privacy-safe, architecturally acceptable, and has meaningful tests where practical. The work may be committed.

`fail`: The work has correctness, privacy, architecture, lifecycle, or test-quality issues. Do not commit. Return it to the responsible engineer with specific findings.
