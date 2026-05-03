using RegionShare.App.Preview;

namespace RegionShare.Tests;

public sealed class PreviewBlackoutControllerTests
{
    [Fact]
    public void RequestBlackoutPublishesEvent()
    {
        var controller = new PreviewBlackoutController();
        var wasPublished = false;
        controller.BlackoutRequested += (_, _) => wasPublished = true;

        controller.RequestBlackout();

        Assert.True(wasPublished);
    }
}
