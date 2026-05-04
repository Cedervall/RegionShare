# RegionShare

RegionShare is a local Windows desktop app for sharing a selected screen region as a normal application window in Microsoft Teams.

The app uses a transparent capture window to define the region, a separate control window for settings, and a Region Share window that Teams can share.

## User-Facing Windows

- **Capture Window**: transparent green region selector. It defines the capture area and can be locked, hidden, resized, snapped, or set from presets.
- **Control Window**: app controls and settings. It starts/stops capture, controls the capture window, configures region size, FPS, cursor capture, preview mode, and overlay status/latency visibility.
- **Region Share Window**: preview/output window intended for Teams sharing. It shows the captured region and resets to a placeholder when capture stops.

The `Snap Region` action temporarily opens a standard resizable setup window so Windows Snap and FancyZones can size the capture region. This setup window is part of the Capture Window workflow, not a primary app window.

## Current Features

- Transparent capture region with lock, hide/show, and click-through locked mode.
- Region Share preview window for Teams window sharing.
- Control window kept separate from the Teams-shared preview.
- Start/stop capture with preview reset on stop.
- Capture FPS options: `30`, `60`, `90`, and `120`.
- Optional cursor capture.
- Borderless preview mode.
- Aspect ratio modes: Free, 16:9, 16:10, and 4:3.
- Aspect-ratio-aware presets.
- Snap Region flow for Windows Snap and FancyZones sizing.
- Optional capture-window status and latency display.
- Direct3D Desktop Duplication capture backend with GDI fallback.
- Local settings persistence under `%LOCALAPPDATA%\RegionShare`.

## Privacy

RegionShare is a local-only desktop app. It must not transmit screen contents, telemetry, settings, logs, crash dumps, or user activity to any remote service unless the user explicitly adds and enables such a feature later.

Captured screen content is not persisted by default. Logs must not include captured screen content or sensitive user activity.

## Development

Build the solution:

```powershell
dotnet build "RegionShare.slnx"
```

Run tests:

```powershell
dotnet test "RegionShare.slnx"
```

Run the app from source:

```powershell
dotnet run --project "src\RegionShare.App\RegionShare.App.csproj"
```

## Packaging

RegionShare can be packaged as a self-contained Windows app and installer. The installer does not require .NET to be installed on the target machine.

Prerequisites for installer creation:

- .NET SDK
- Inno Setup 6

Build the installer from a version tag such as `v0.1.2`:

```powershell
.\scripts\package.ps1
```

Or pass a version explicitly:

```powershell
.\scripts\package.ps1 -Version 0.1.2
```

Installer output:

```text
artifacts\installer\RegionShareSetup-0.1.2.exe
```

See `docs/packaging.md` for release and installer details.

## Workflow

Development follows `docs/workflow.md`. Every ticket requires meaningful tests where practical, a passing `dotnet test`, and reviewer `pass` before commit.
