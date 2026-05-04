# Footer Links And Version

## Goal
Add useful footer links and version metadata to the control window once RegionShare has public resources.

## Tasks
- Add a website link when a RegionShare website exists.
- Add documentation and support links when those pages exist.
- Display the app version from assembly metadata instead of hardcoded text. Completed.
- Keep links local-user initiated only; do not add telemetry or network calls.

## Acceptance Criteria
- Footer links are visible only for configured real URLs.
- Clicking a footer link opens the user's browser.
- App version is read from assembly/package metadata.
- Tests cover link visibility/formatting state where practical.

## Status
Partially completed.

## Verification
- Control Window displays the app version from assembly metadata.
- Version formatting is covered by tests.
- Website, documentation, and support links remain in the backlog until real public URLs exist.
