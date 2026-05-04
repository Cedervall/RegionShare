# Packaging

RegionShare is packaged as a self-contained Windows app and an Inno Setup installer.

The installer does not require .NET to be installed on the target machine.

## Prerequisites

- .NET SDK
- Inno Setup 6

Optional:

- ImageMagick, only if you want `scripts/package.ps1` to convert `RegionShare.png` into `RegionShare.ico` automatically.

## App Icon

The app and installer use:

```text
src\RegionShare.App\Assets\RegionShare.ico
```

The source PNG and individual icon sizes are also kept in the same folder.

## Versioning

Release tags use semantic versioning with a `v` prefix:

```text
v0.1.2
```

When `scripts/package.ps1` is run without `-Version`, it reads the exact current git tag and expects it to match `vMAJOR.MINOR.PATCH`.

You can also pass the version explicitly:

```powershell
.\scripts\package.ps1 -Version 0.1.2
```

## Build Installer

From the repository root:

```powershell
.\scripts\package.ps1
```

The script:

- cleans `artifacts/`
- publishes `RegionShare.App` as self-contained `win-x64`
- builds a single-file app executable
- invokes Inno Setup
- writes the installer to `artifacts\installer`
- writes a SHA-256 checksum file next to the installer

Expected installer output for version `0.1.2`:

```text
artifacts\installer\RegionShareSetup-0.1.2.exe
artifacts\installer\RegionShareSetup-0.1.2.exe.sha256
```

## Verify Installer Integrity

Each installer build writes a `.sha256` file next to the installer. The checksum verifies that a downloaded installer matches the file produced during packaging.

On Windows, users can calculate the installer hash with:

```powershell
Get-FileHash ".\RegionShareSetup-0.1.2.exe" -Algorithm SHA256
```

Compare the output hash to:

```text
RegionShareSetup-0.1.2.exe.sha256
```

Checksum verification confirms file integrity. It does not replace code signing or prove publisher identity.

## Publish Without Installer

To create only the self-contained app folder:

```powershell
.\scripts\package.ps1 -Version 0.1.2 -SkipInstaller
```

Output:

```text
artifacts\publish\RegionShare\RegionShare.exe
```

## Installer Behavior

- Per-user install.
- No admin rights required.
- Installs under local app data programs folder.
- Creates a Start Menu shortcut.
- Offers an optional desktop shortcut.
- Offers to launch RegionShare after install.
- Excludes `.pdb` debug symbols from the installer package.

## Code Signing

The installer is currently unsigned.

Unsigned installers can show Windows “Unknown publisher” or SmartScreen warnings. This does not prevent local testing, but code signing should be added before broad public distribution.

## Release Checklist

1. Ensure working tree is clean.
2. Update project and installer fallback versions if needed.
3. Run `dotnet build "RegionShare.slnx"`.
4. Run `dotnet test "RegionShare.slnx"`.
5. Commit release changes.
6. Create a semantic version tag, for example `v0.1.2`.
7. Run `.\scripts\package.ps1` from that tag. Passing `-Version` can package a dirty tree, so only use it for local testing unless the working tree is clean.
8. Confirm `artifacts\installer\RegionShareSetup-<version>.exe.sha256` was created.
9. Smoke-test `artifacts\installer\RegionShareSetup-<version>.exe` on a Windows machine.
