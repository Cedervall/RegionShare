# RegionShare

RegionShare is a local Windows desktop app for sharing a selected part of your screen in online meetings.

It turns a screen region into a normal application window, so you can share only the area you choose instead of an entire monitor or application.

## Highlights

- Select and share a specific screen region.
- Share the selected region as a normal app window in meeting software.
- GPU-powered capture for a smoother feel at higher frame rates when supported.
- Local-only operation with no telemetry or network sharing.
- Self-contained Windows installer with no .NET requirement.

## Download And Install

Download the latest installer from GitHub Releases.

The installer is self-contained. You do not need to install .NET separately.

Installer artifact:

```text
RegionShareSetup-0.1.4.exe
```

The installer is currently unsigned. Windows may show an `Unknown publisher` or SmartScreen warning until code signing is added.

## Verify The Installer

Each installer release includes a SHA-256 checksum file:

```text
RegionShareSetup-0.1.4.exe.sha256
```

To calculate the hash locally on Windows:

```powershell
Get-FileHash ".\RegionShareSetup-0.1.4.exe" -Algorithm SHA256
```

Compare the output with the contents of the `.sha256` file to verify the installer has not been changed.

Checksum verification confirms file integrity. It does not replace code signing or prove publisher identity.

## How To Use

1. Open RegionShare.
2. Position the Capture Window over the part of the screen you want to share.
3. Click `Start Capture` in the Control Window.
4. Share the Region Share Window in your meeting software.
5. Click `Stop Capture` when done.

## Privacy

RegionShare is a local-only app. It does not send screen contents, telemetry, settings, logs, crash reports, or user activity to any remote service.

RegionShare will never capture and share information outside your machine. The only sharing that happens is what you explicitly choose to share through your meeting software.

Captured screen content is not persisted by RegionShare.

## Requirements

- Windows 10 or Windows 11.
- x64 Windows.
- No separate .NET installation required when using the installer.

## Troubleshooting

- If cursor capture does not apply immediately, stop capture and start it again.
- If Windows shows `Unknown publisher`, the installer is currently unsigned.
- If capture is not smooth, lower the capture FPS or disable cursor capture.
- If the Capture Window is locked and click-through, use the Control Window or `Ctrl + Alt + L` to unlock it.

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

RegionShare can be packaged as a self-contained Windows app and installer.

Prerequisites for installer creation:

- .NET SDK
- Inno Setup 6

Build the installer from a version tag such as `v0.1.4`:

```powershell
.\scripts\package.ps1
```

Or pass a version explicitly:

```powershell
.\scripts\package.ps1 -Version 0.1.4
```

Installer output:

```text
artifacts\installer\RegionShareSetup-0.1.4.exe
artifacts\installer\RegionShareSetup-0.1.4.exe.sha256
```

See `docs/packaging.md` for release and installer details.

## Workflow

Development follows `docs/workflow.md`. Every ticket requires meaningful tests where practical, a passing `dotnet test`, and reviewer `pass` before commit.
