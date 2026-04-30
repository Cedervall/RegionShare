namespace RegionShare.App.Capture;

public static class DesktopOutputRegionMapper
{
    public static MappedOutputRegion? Map(CaptureRegion region, IReadOnlyList<DesktopOutputBounds> outputs)
    {
        for (var index = 0; index < outputs.Count; index++)
        {
            var output = outputs[index];
            if (Contains(output, region))
            {
                return new MappedOutputRegion(index, new CaptureRegion(region.X - output.Left, region.Y - output.Top, region.Width, region.Height));
            }
        }

        return null;
    }

    private static bool Contains(DesktopOutputBounds output, CaptureRegion region)
    {
        return region.X >= output.Left
            && region.Y >= output.Top
            && region.X + region.Width <= output.Right
            && region.Y + region.Height <= output.Bottom;
    }
}
