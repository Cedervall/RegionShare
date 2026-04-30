# Performance Validation

The app targets approximately 30 FPS for the current local GDI capture backend. The capture timer interval is about 33 ms.

## Manual Checks
- Start capture and leave it running for at least 15 minutes.
- Confirm CPU usage remains acceptable for meeting use.
- Confirm memory does not grow continuously.
- Toggle start/stop repeatedly and confirm capture recovers.
- Hide/show and lock/unlock the overlay while capture is running.
- Confirm closing the preview exits all windows and releases the app process.

## Privacy
Do not capture, save, upload, or log frame contents during performance validation.
