# Bootstrap Repository

## Goal
Initialize the folder as a git repository and create the base .NET solution structure.

## Tasks
- Initialize git repository.
- Add `.gitignore` suitable for .NET/WPF.
- Create solution file.
- Create WPF app project.
- Create xUnit test project.
- Add initial README with product goal and MVP scope.

## Acceptance Criteria
- Repository is initialized.
- Solution builds.
- Test project exists.
- `dotnet test` passes.
- Git status shows intentional project files only.

## Status
Completed in commit `be37cb2`.

## Verification
- `dotnet build "RegionShare.slnx"` passed.
- `dotnet test "RegionShare.slnx"` passed.
- Reviewer outcome: `pass`.
