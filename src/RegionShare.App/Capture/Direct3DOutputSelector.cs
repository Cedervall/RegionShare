namespace RegionShare.App.Capture;

using Vortice.DXGI;

public static class Direct3DOutputSelector
{
    public static Direct3DOutputSelection? Select(IDXGIFactory1 factory, CaptureRegion region)
    {
        for (uint adapterIndex = 0; factory.EnumAdapters1(adapterIndex, out var adapter).Success; adapterIndex++)
        {
            var outputBounds = new List<DesktopOutputBounds>();
            var outputs = new List<IDXGIOutput>();
            for (uint outputIndex = 0; adapter.EnumOutputs(outputIndex, out var output).Success; outputIndex++)
            {
                if (output.Description.AttachedToDesktop)
                {
                    var coordinates = output.Description.DesktopCoordinates;
                    outputBounds.Add(new DesktopOutputBounds(coordinates.Left, coordinates.Top, coordinates.Right, coordinates.Bottom));
                    outputs.Add(output);
                }
                else
                {
                    output.Dispose();
                }
            }

            var mappedRegion = DesktopOutputRegionMapper.Map(region, outputBounds);
            if (mappedRegion is not null)
            {
                for (var index = 0; index < outputs.Count; index++)
                {
                    if (index != mappedRegion.OutputIndex)
                    {
                        outputs[index].Dispose();
                    }
                }

                return new Direct3DOutputSelection(adapter, outputs[mappedRegion.OutputIndex], mappedRegion);
            }

            foreach (var output in outputs)
            {
                output.Dispose();
            }

            adapter.Dispose();
        }

        return null;
    }
}
