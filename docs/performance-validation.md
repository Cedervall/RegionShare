# Performance Validation

RegionShare supports `30`, `60`, `90`, and `120` capture FPS settings. The default backend is Direct3D Desktop Duplication when supported; GDI is used as fallback and for cursor capture.

## Manual Checks

- Test capture at `30`, `60`, `90`, and `120` FPS.
- Confirm CPU usage remains acceptable for meeting use at each FPS setting.
- Confirm memory does not grow continuously during at least 15 minutes of capture.
- Toggle start/stop repeatedly and confirm capture recovers.
- Hide/show and lock/unlock the Capture Window while capture is running.
- Toggle cursor capture while running and confirm the Control Window asks for a capture restart.
- Restart capture with cursor capture enabled and confirm cursor rendering works via the GDI backend.
- Confirm the optional latency/frame interval overlay updates while capture is running.
- Confirm closing the Region Share Window exits all windows and releases the app process.

## Privacy

Do not capture, save, upload, or log frame contents during performance validation.
