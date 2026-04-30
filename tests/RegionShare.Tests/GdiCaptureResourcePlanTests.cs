using RegionShare.App.Capture;

namespace RegionShare.Tests;

public sealed class GdiCaptureResourcePlanTests
{
    [Fact]
    public void ShouldRecreateBitmapReturnsFalseWhenDimensionsMatch()
    {
        var shouldRecreate = GdiCaptureResourcePlan.ShouldRecreateBitmap(1280, 720, new CaptureRegion(100, 100, 1280, 720));

        Assert.False(shouldRecreate);
    }

    [Theory]
    [InlineData(1279, 720)]
    [InlineData(1280, 719)]
    public void ShouldRecreateBitmapReturnsTrueWhenDimensionsChange(int currentWidth, int currentHeight)
    {
        var shouldRecreate = GdiCaptureResourcePlan.ShouldRecreateBitmap(currentWidth, currentHeight, new CaptureRegion(100, 100, 1280, 720));

        Assert.True(shouldRecreate);
    }
}
