# Add Settings Persistence

## Goal
Remember user preferences between launches.

## Tasks
- Persist last overlay position and size.
- Persist lock state.
- Persist overlay visibility state.
- Persist preview window position and size.
- Persist control window position and size.
- Persist preview borderless mode.
- Persist aspect ratio mode.
- Persist cursor capture enabled/disabled when cursor capture is added.
- Persist preview always-on-top preference if added.
- Store settings in local app data.
- Do not persist whether capture was running.
- Do not persist captured screen content, cursor positions, screenshots, or user activity.

## Acceptance Criteria
- App restores previous region on launch.
- App restores preview and control window size/position on launch.
- App restores preview borderless mode on launch.
- App starts with capture stopped even if capture was active in the previous session.
- Settings survive app restart.
- Invalid settings are handled safely.
- Tests cover serialization, default settings, invalid settings fallback, and exclusion of capture-running/screen-content state.
- `dotnet test` passes.
