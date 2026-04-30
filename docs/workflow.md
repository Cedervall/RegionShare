# Development Workflow

Every ticket follows the same implementation and review flow.

## Ticket Flow
1. Assign the ticket to the correct engineer.
2. Implement the smallest correct change.
3. Add or update meaningful tests where practical.
4. Run `dotnet test`.
5. Reviewer checks code correctness, privacy, architecture, lifecycle safety, and test quality.
6. If the reviewer returns `pass`, commit the work.
7. If the reviewer returns `fail`, do not commit. Return the work to the responsible engineer with specific findings, fix the issues, rerun tests, and review again.

## Assignment Guide
- Use `application-engineer` for WPF shell, overlay behavior, preview window shell, app state, settings, presets, aspect ratios, lock mode, and non-platform hotkey behavior.
- Use `capture-platform-engineer` for Windows Graphics Capture, frame rendering, DPI conversion, monitor coordinates, Win32 interop, overlay capture exclusion, global hotkey registration, resource disposal, and performance.
- Use `reviewer` for every completed ticket before commit.

## Commit Gate
No work may be committed unless all of the following are true:
- The assigned engineer completed the ticket.
- Meaningful tests were added or updated where practical.
- `dotnet test` passes.
- The reviewer explicitly returns `pass`.

## Meaningful Tests
A test is meaningful when it protects real behavior. It should fail if the feature, fix, or important edge case regresses.

Reject tests that only prove code can be instantiated, duplicate implementation logic, verify mocks instead of project behavior, or would pass if the feature were removed.

Prefer tests that assert observable behavior, state transitions, invalid input handling, lifecycle cleanup, and edge cases central to the ticket.
