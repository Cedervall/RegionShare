# Code Sign Installer

## Goal
Sign the RegionShare installer before broader public distribution so Windows can identify the publisher.

## Tasks
- Choose a code-signing certificate and signing provider.
- Add signing to the release packaging flow after installer creation.
- Keep SHA-256 checksum generation after signing so the checksum matches the distributed installer.
- Document certificate requirements and local/CI signing steps.
- Verify Windows no longer shows `Unknown publisher` for signed builds.

## Acceptance Criteria
- Release installers are signed with the RegionShare publisher identity.
- The generated `.sha256` file matches the signed installer.
- Packaging docs explain how signing is performed.
- Unsigned local test builds remain possible when no signing certificate is configured.
