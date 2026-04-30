# Region Share

Region Share is a Windows desktop application for sharing a selected screen region as a normal application window in Microsoft Teams.

The app uses a movable transparent overlay to define the capture area and a separate preview window that Teams can share.

## MVP Scope

- Transparent overlay window
- Move and resize region selection
- Lock and unlock region
- Live preview window
- Region capture suitable for Teams window sharing
- DPI-aware region mapping
- Tests passing for every task and feature

## Privacy

Region Share is a local-only desktop app. It must not transmit screen contents, telemetry, settings, logs, crash dumps, or user activity to any remote service unless the user explicitly adds and enables such a feature later.

Captured screen content should not be persisted by default. Logs must not include captured screen content or sensitive user activity.

## Development

Build the solution:

```powershell
dotnet build "RegionShare.slnx"
```

Run tests:

```powershell
dotnet test "RegionShare.slnx"
```

## Workflow

Development follows `docs/workflow.md`. Every ticket requires meaningful tests where practical, a passing `dotnet test`, and reviewer `pass` before commit.
