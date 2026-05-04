# Capture Exclusion

The Capture Window uses `SetWindowDisplayAffinity` with `WDA_EXCLUDEFROMCAPTURE` to request that Windows excludes the selector window from screen capture.

## Current Backends

The default backend is `Direct3DDesktopDuplicationScreenCaptureService` when supported and cursor capture is disabled.

`GdiScreenCaptureService` is used as fallback and when cursor capture is enabled.

Windows display-affinity support can vary by OS version, capture API, graphics driver, and capture path. The app treats Capture Window exclusion as required behavior, but it must be manually validated on the target machine with the active backend.

## Expected Result

- The capture border, status labels, latency label, and resize handles must not appear in the Region Share Window.
- The Region Share Window should show the desktop and windows underneath the selected region.
- Capture remains local-only and does not transmit or persist screen contents.

## Manual Validation

1. Run the app.
2. Place the Capture Window over visible desktop/application content.
3. Click `Start Capture` in the Control Window.
4. Confirm the Region Share Window does not contain the Capture Window border, controls, or labels.
5. Lock the Capture Window and confirm the Region Share Window still excludes it.
6. Hide/show the Capture Window and confirm capture continues from the selected region.
7. Enable cursor capture, restart capture when prompted, and repeat the exclusion check with the GDI backend.

If the Capture Window appears in the Region Share Window, treat it as a release-blocking bug for the active backend.
