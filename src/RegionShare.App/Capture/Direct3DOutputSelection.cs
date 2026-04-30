namespace RegionShare.App.Capture;

using Vortice.DXGI;

public sealed class Direct3DOutputSelection : IDisposable
{
    public Direct3DOutputSelection(IDXGIAdapter1 adapter, IDXGIOutput output, MappedOutputRegion mappedRegion)
    {
        Adapter = adapter;
        Output = output;
        MappedRegion = mappedRegion;
    }

    public IDXGIAdapter1 Adapter { get; }

    public IDXGIOutput Output { get; }

    public MappedOutputRegion MappedRegion { get; }

    public void Dispose()
    {
        Output.Dispose();
        Adapter.Dispose();
    }
}
