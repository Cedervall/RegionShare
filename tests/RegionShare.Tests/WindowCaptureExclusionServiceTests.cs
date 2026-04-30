using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class WindowCaptureExclusionServiceTests
{
    [Fact]
    public void ExcludeHandleFromCaptureReturnsFalseForZeroHandleWithoutCallingWin32Boundary()
    {
        var wasCalled = false;
        var service = new WindowCaptureExclusionService((_, _) =>
        {
            wasCalled = true;
            return true;
        });

        var result = service.ExcludeHandleFromCapture(IntPtr.Zero);

        Assert.False(result);
        Assert.False(wasCalled);
    }

    [Fact]
    public void ExcludeHandleFromCapturePassesExpectedAffinityToWin32Boundary()
    {
        var handle = new IntPtr(123);
        IntPtr? capturedHandle = null;
        uint? capturedAffinity = null;
        var service = new WindowCaptureExclusionService((providedHandle, providedAffinity) =>
        {
            capturedHandle = providedHandle;
            capturedAffinity = providedAffinity;
            return true;
        });

        var result = service.ExcludeHandleFromCapture(handle);

        Assert.True(result);
        Assert.Equal(handle, capturedHandle);
        Assert.Equal(WindowCaptureExclusionService.ExcludeFromCaptureAffinity, capturedAffinity);
    }

    [Fact]
    public void ExcludeHandleFromCaptureReturnsFalseWhenWin32BoundaryFails()
    {
        var service = new WindowCaptureExclusionService((_, _) => false);

        var result = service.ExcludeHandleFromCapture(new IntPtr(123));

        Assert.False(result);
    }
}
