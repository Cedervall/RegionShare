# Capture Exclusion

The overlay uses `SetWindowDisplayAffinity` with `WDA_EXCLUDEFROMCAPTURE` to request that Windows excludes the selector window from screen capture.

## Current Backend
The current MVP capture backend is `GdiScreenCaptureService`, which captures the selected screen rectangle locally using GDI `BitBlt`.

Windows display-affinity support can vary by OS version, capture API, graphics driver, and capture path. The app treats overlay exclusion as a required behavior, but it must be manually validated on the target machine with the active capture backend.

## Expected Result
- The overlay border, size label, lock controls, and resize handles must not appear in the preview image.
- The preview should show the desktop and windows underneath the selected region.
- Capture remains local-only and does not transmit or persist screen contents.

## Manual Validation
1. Run the app.
2. Place the overlay over visible desktop/application content.
3. Click `Start capture` in the preview window.
4. Confirm the preview does not contain the overlay border, controls, or label.
5. Lock the overlay and confirm the preview still excludes the overlay.
6. Hide/show the overlay and confirm capture continues from the selected region.

If the overlay appears in the preview, prioritize replacing the interim GDI backend with Windows Graphics Capture or another capture path that reliably honors exclusion.
