# Add Settings Persistence

## Goal
Remember user preferences between launches.

## Tasks
- Persist last overlay position and size.
- Persist lock state.
- Persist aspect ratio mode.
- Persist preview always-on-top preference if added.
- Store settings in local app data.

## Acceptance Criteria
- App restores previous region on launch.
- Settings survive app restart.
- Invalid settings are handled safely.
- Tests cover serialization, default settings, and invalid settings fallback.
- `dotnet test` passes.
