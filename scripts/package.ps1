[CmdletBinding()]
param(
    [string]$Version,
    [string]$Runtime = "win-x64",
    [string]$Configuration = "Release",
    [switch]$SkipInstaller
)

$ErrorActionPreference = "Stop"

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot "..")
$projectPath = Join-Path $repoRoot "src\RegionShare.App\RegionShare.App.csproj"
$assetsDir = Join-Path $repoRoot "src\RegionShare.App\Assets"
$iconPng = Join-Path $assetsDir "RegionShare.png"
$iconIco = Join-Path $assetsDir "RegionShare.ico"
$publishDir = Join-Path $repoRoot "artifacts\publish\RegionShare"
$installerDir = Join-Path $repoRoot "artifacts\installer"
$innoScript = Join-Path $repoRoot "installer\RegionShare.iss"

if ([string]::IsNullOrWhiteSpace($Version)) {
    $tag = (& git -C $repoRoot describe --tags --exact-match 2>$null)
    if ($LASTEXITCODE -ne 0 -or [string]::IsNullOrWhiteSpace($tag)) {
        throw "No exact git tag found. Create a version tag like 'v0.1.2' or pass -Version 0.1.2."
    }

    if ($tag -notmatch '^v(?<version>\d+\.\d+\.\d+)$') {
        throw "Git tag '$tag' must match vMAJOR.MINOR.PATCH, for example v0.1.2."
    }

    $Version = $Matches.version
}

if ($Version -notmatch '^\d+\.\d+\.\d+$') {
    throw "Version '$Version' must match MAJOR.MINOR.PATCH, for example 0.1.2."
}

if ((Test-Path $iconPng) -and !(Test-Path $iconIco)) {
    $magick = Get-Command magick -ErrorAction SilentlyContinue
    if ($magick) {
        & $magick.Source $iconPng -define icon:auto-resize=256,128,64,48,32,16 $iconIco
        if ($LASTEXITCODE -ne 0) {
            throw "ImageMagick failed to convert RegionShare.png to RegionShare.ico."
        }
    } else {
        Write-Warning "RegionShare.png exists but RegionShare.ico is missing. Install ImageMagick or provide RegionShare.ico for app/installer icons."
    }
}

Remove-Item (Join-Path $repoRoot "artifacts") -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Path $publishDir | Out-Null
New-Item -ItemType Directory -Path $installerDir | Out-Null

dotnet publish $projectPath `
    -c $Configuration `
    -r $Runtime `
    --self-contained true `
    -p:PublishSingleFile=true `
    -p:IncludeNativeLibrariesForSelfExtract=true `
    -p:Version=$Version `
    -p:FileVersion="$Version.0" `
    -p:ProductVersion=$Version `
    -o $publishDir

if ($LASTEXITCODE -ne 0) {
    throw "dotnet publish failed."
}

if ($SkipInstaller) {
    Write-Host "Published self-contained app to $publishDir"
    return
}

$iscc = Get-Command ISCC.exe -ErrorAction SilentlyContinue
if (-not $iscc) {
    $defaultIsccPaths = @(
        "${env:ProgramFiles(x86)}\Inno Setup 6\ISCC.exe",
        "$env:ProgramFiles\Inno Setup 6\ISCC.exe",
        "$env:LOCALAPPDATA\Programs\Inno Setup 6\ISCC.exe"
    )
    $isccPath = $defaultIsccPaths | Where-Object { Test-Path $_ } | Select-Object -First 1
    if ($isccPath) {
        $iscc = Get-Item $isccPath
    }
}

if (-not $iscc) {
    throw "Inno Setup compiler (ISCC.exe) was not found. Install Inno Setup 6, then rerun this script. Published app is available at $publishDir."
}

& $iscc.FullName "/DMyAppVersion=$Version" "/DPublishDir=$publishDir" $innoScript
if ($LASTEXITCODE -ne 0) {
    throw "Inno Setup compiler failed."
}

$installerPath = Join-Path $installerDir "RegionShareSetup-$Version.exe"
$checksumPath = "$installerPath.sha256"
$checksum = Get-FileHash $installerPath -Algorithm SHA256
"$($checksum.Hash.ToLowerInvariant())  $(Split-Path $installerPath -Leaf)" | Set-Content -Path $checksumPath -Encoding ascii

Write-Host "Installer created at $installerPath"
Write-Host "SHA-256 checksum created at $checksumPath"
